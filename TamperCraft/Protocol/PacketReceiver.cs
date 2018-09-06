using Craft.Net.Packets;
using Craft.Net.Packets.Login;
using Craft.Net.Packets.Play;
using Craft.Net.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Craft.Net
{
    public class PacketReceiver
    {
        private IPacket[] loginPackets =
        {
            new S00LoginRejected(),
            new S01EncryptionRequest(),
            new S02LoginSuccessful(),
            new S03Compression()
        };

        private IPacket[] playPackets =
        {
            new S00KeepAlive(),
            new S01JoinCompleted(),
            new S02ChatMessage(),
            new S06HealthChanged(),
            new S08PlayerPosition(),
            new S40Disconnect(),
            new S2FSlotUpdate(),
            new S0CPlayerJoin()
        };

        private NetHandler netHandler;
        private MinecraftConnection manager;

        public PacketReceiver(MinecraftConnection manager, NetHandler netHandler)
        {
            this.manager = manager;
            this.netHandler = netHandler;
        }

        public void StartReceiver()
        {
            (new Thread(() =>
            {
                while (true)
                {
                    int packetID = -1;

                    PacketBuffer buffer = ReadNextPacket(ref packetID);
                    if (manager.gameState == GameState.Login)
                    {
                        foreach (IPacket packet in loginPackets)
                        {
                            if (packet.GetId() == packetID)
                            {
                                packet.Receive(buffer);
                                packet.Handle(netHandler);
                            }
                        }
                    }
                    else manager.Interceptor.OnServerPacket(packetID, buffer);
                }
            })).Start();
        }

        private PacketBuffer ReadNextPacket(ref int packetID)
        {
            int size = ReadNextVarIntRAW();
            var b = ReadDataRAW(size);
            PacketBuffer buffer = new PacketBuffer(b);

            if (manager.compressionThreshold > 0)
            {
                int sizeUncompressed = buffer.ReadVarInt();
                if (sizeUncompressed != 0) // != 0 means compressed, let's decompress
                {
                    buffer = new PacketBuffer(Zlib.Decompress(buffer.ReadToEnd(), sizeUncompressed));
                }
            }
            packetID = buffer.ReadVarInt();
            stream = new MemoryStream();
            return buffer;
        }

        private MemoryStream stream = new MemoryStream();

        private int ReadNextVarIntRAW()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            byte[] tmp = new byte[1];
            while (true)
            {
                Receive(tmp, 0, 1);
                stream.Write(tmp, 0, 1);
                k = tmp[0];
                i |= (k & 0x7F) << j++ * 7;
                if (j > 5) throw new OverflowException("VarInt too big");
                if ((k & 0x80) != 128) break;
            }
            return i;
        }

        private byte[] ReadDataRAW(int offset)
        {
            if (offset > 0)
            {
                try
                {
                    byte[] cache = new byte[offset];
                    Receive(cache, 0, offset);
                    return cache;
                }
                catch (OutOfMemoryException) { }
            }
            return new byte[] { };
        }

        private void Receive(byte[] buffer, int start, int offset)
        {
            int read = 0;
            while (read < offset)
            {
                if (manager.encrypted)
                {
                    read += manager.aesStream.Read(buffer, start + read, offset - read);
                }
                else read += manager.tcpClient.Client.Receive(buffer, start + read, offset - read, System.Net.Sockets.SocketFlags.None);
            }
        }
    }
}
