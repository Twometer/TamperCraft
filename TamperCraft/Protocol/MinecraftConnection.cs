using Craft.Net.Crypto;
using Craft.Net.Packets;
using Craft.Net.Util;
using System;
using System.Net.Sockets;
using System.Threading;
using TamperCraft.Protocol.Packets;

namespace Craft.Net
{
    public class MinecraftConnection
    {
        public TcpClient tcpClient;
        public LoginParams loginParams;
        public PacketReceiver receiver;
        public NetHandler netHandler;

        public GameState gameState = GameState.Login;
        public bool encrypted;
        public int compressionThreshold;

        public IPacketInterceptor Interceptor { get; set; }

        public IAesStream aesStream;

        public bool Login(LoginParams loginParams)
        {
            this.loginParams = loginParams;

            try
            {
                tcpClient = new TcpClient(loginParams.ServerHost, loginParams.ServerPort)
                {
                    ReceiveBufferSize = 1024 * 1024
                };
                SendPacket(new C00PacketHandshake(47, loginParams.ServerHost, loginParams.ServerPort, 2));
                SendPacket(new C00PacketLogin(loginParams.Username));

                netHandler = new NetHandler(this);
                receiver = new PacketReceiver(this, netHandler);
                receiver.StartReceiver();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void SendBytes(byte[] array)
        {
            if (encrypted)
            {
                aesStream.Write(array, 0, array.Length);
            }
            else
            {
                tcpClient.Client.Send(array);
            }
        }

        public void SendPacket(int id, PacketBuffer buffer)
        {
            byte[] packetIdVI = ByteUtils.ToVarInt(id);
            byte[] packetData = ByteUtils.Concat(packetIdVI, buffer.ReadToEnd());
            if (compressionThreshold > 0)
            {
                if (packetData.Length > compressionThreshold)
                {
                    byte[] uncompressed_length = ByteUtils.ToVarInt(packetData.Length);
                    byte[] compressed_packet = Zlib.Compress(packetData);
                    byte[] compressed_packet_length = ByteUtils.ToVarInt(compressed_packet.Length);
                    packetData = ByteUtils.Concat(compressed_packet_length, compressed_packet);
                }
                else
                {
                    byte[] uncompressed_length = ByteUtils.ToVarInt(0);
                    packetData = ByteUtils.Concat(uncompressed_length, packetData);
                }
            }

            byte[] lengthVI = ByteUtils.ToVarInt(packetData.Length);
            var l = ByteUtils.Concat(lengthVI, packetData);
            SendBytes(l);
        }

        public void SendPacket(IPacket packet)
        {
            PacketBuffer sendBuffer = new PacketBuffer();
            packet.Send(sendBuffer);
            byte[] packetIdVI = ByteUtils.ToVarInt(packet.GetId());
            byte[] packetData = ByteUtils.Concat(packetIdVI, sendBuffer.ToArray());
            if (compressionThreshold > 0)
            {
                if (packetData.Length > compressionThreshold)
                {
                    byte[] uncompressed_length = ByteUtils.ToVarInt(packetData.Length);
                    byte[] compressed_packet = Zlib.Compress(packetData);
                    byte[] compressed_packet_length = ByteUtils.ToVarInt(compressed_packet.Length);
                    packetData = ByteUtils.Concat(compressed_packet_length, compressed_packet);
                }
                else
                {
                    byte[] uncompressed_length = ByteUtils.ToVarInt(0);
                    packetData = ByteUtils.Concat(uncompressed_length, packetData);
                }
            }

            byte[] lengthVI = ByteUtils.ToVarInt(packetData.Length);
            var l = ByteUtils.Concat(lengthVI, packetData);
            SendBytes(l);
        }

        public void WaitForConnect()
        {
            while (!encrypted) ;
        }

        public void Close()
        {
            tcpClient.Close();
        }
    }
}