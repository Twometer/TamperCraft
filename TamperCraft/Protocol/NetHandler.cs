using Craft.Net.Auth;
using Craft.Net.Crypto;
using Craft.Net.Packets.Login;
using Craft.Net.Packets.Play;
using System.Security.Cryptography;
using System;

namespace Craft.Net
{
    public class NetHandler
    {
        public MinecraftConnection manager;

        public void HandleS2FSlotUpdate(S2FSlotUpdate s2FSlotUpdate)
        {
     
        }

        public void HandleS0CPlayerJoin(S0CPlayerJoin s0CPlayerJoin)
        {

        }

        public bool waitingForEncryption;

        public NetHandler(MinecraftConnection manager)
        {
            this.manager = manager;
        }



        internal void HandleS00KeepAlive(S00KeepAlive s00KeepAlive)
        {
            manager.SendPacket(new C00KeepAlive(s00KeepAlive.id));
        }

        internal void HandleS00LoginRejected(S00LoginRejected s00LoginRejected)
        {
            Console.WriteLine("Login rejected: " + s00LoginRejected.Reason);
        }

        internal void HandleS01EncryptionRequest(S01EncryptionRequest s01EncryptionRequest)
        {
            waitingForEncryption = true;
            Console.WriteLine("Server is in online mode!");
            RSACryptoServiceProvider rsaProvider = CryptoHandler.DecodeRSAPublicKey(s01EncryptionRequest.serverKey);
            byte[] secretKey = CryptoHandler.GenerateAESPrivateKey();

            Console.WriteLine("Keys generated");
            if (s01EncryptionRequest.serverId != "-")
            {
                Console.WriteLine("Logging in to Mojang");
                if (!Yggdrasil.SessionCheck(manager.loginParams.UUID, manager.loginParams.SessionId, CryptoHandler.getServerHash(s01EncryptionRequest.serverId, s01EncryptionRequest.serverKey, secretKey)))
                {
                    Console.WriteLine("Mojang authentication failed");
                    return;
                }
                else
                {
                    Console.WriteLine("Mojang authentication succesful");
                }
            }

            manager.SendPacket(new C01EncryptionResponse(rsaProvider.Encrypt(secretKey, false), rsaProvider.Encrypt(s01EncryptionRequest.token, false)));

            manager.aesStream = CryptoHandler.getAesStream(manager.tcpClient.GetStream(), secretKey);
            manager.encrypted = true;
        }

        internal void HandleS01JoinCompleted(S01JoinCompleted s01JoinCompleted)
        {

        }

        internal void HandleS02ChatMessage(S02ChatMessage s02ChatMessage)
        {
        }

        internal void HandleS02LoginSuccesful(S02LoginSuccessful s02LoginSuccessful)
        {

            manager.gameState = GameState.Play;
        }

        internal void HandleS03Compression(S03Compression s03Compression)
        {
            this.manager.compressionThreshold = s03Compression.Threshold;
            Console.WriteLine("Set compression to " + s03Compression.Threshold);
        }

        internal void HandleS06HealthChanged(S06HealthChanged s06HealthChanged)
        {

        }

        internal void HandleS08PlayerPosition(S08PlayerPosition s08PlayerPosition)
        {

        }

        internal void HandleS40Disconnect(S40Disconnect s40Disconnect)
        {

        }
    }
}