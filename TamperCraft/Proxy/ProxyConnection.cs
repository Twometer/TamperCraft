using Craft.Net.Packets;
using Craft.Net.Util;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using TamperCraft.Protocol.Packets;

namespace TamperCraft.Proxy
{
    public class ProxyConnection
    {
        private TcpClient tcpClient;

        public int CompressionThreshold { get; set; }
        public IPacketInterceptor Interceptor { private get; set; }
        public bool Disconnected { get; private set; }

        public ProxyConnection(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            StartReaderLoop();
        }

        private void StartReaderLoop()
        {
            var thread = new Thread(() =>
            {
                while (!Disconnected)
                {
                    var buf = ReadNextPacket(out int packetId);
                    Interceptor?.OnClientPacket(packetId, buf);
                }
            });
            thread.Start();
        }

        private PacketBuffer ReadNextPacket(out int packetId)
        {
            var size = ReadNextVarInt();
            var buffer = new PacketBuffer(Receive(size));
            if (CompressionThreshold > 0)
            {
                int sizeUncompressed = buffer.ReadVarInt();
                if (sizeUncompressed != 0)
                {
                    buffer = new PacketBuffer(Zlib.Decompress(buffer.ReadToEnd(), sizeUncompressed));
                }
            }
            packetId = buffer.ReadVarInt();
            return buffer;
        }

        private int ReadNextVarInt()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            byte[] tmp = new byte[1];
            while (true)
            {
                Receive(tmp, 0, 1);
                k = tmp[0];
                i |= (k & 0x7F) << j++ * 7;
                if (j > 5) throw new OverflowException("VarInt too big");
                if ((k & 0x80) != 128) break;
            }
            return i;
        }

        public void SendPacket(IPacket packet)
        {
            var buf = new PacketBuffer();
            packet.Send(buf);
            SendPacket(packet.GetId(), buf, true);
        }

        public void SendPacket(int id, PacketBuffer sendBuffer, bool flipBeforeSend = false)
        {
            if (Disconnected) return;
            if (flipBeforeSend) sendBuffer.Reset();
            try
            {
                if (CompressionThreshold == 0)
                {
                    var buffer = new PacketBuffer();
                    buffer.WriteVarInt(id);
                    buffer.WriteRawByteArray(sendBuffer.ReadToEnd());
                    var packetData = buffer.ToArray();
                    Send(ByteUtils.ToVarInt(packetData.Length).Concat(packetData));
                }
                else
                {
                    var buffer = new PacketBuffer();
                    buffer.WriteVarInt(id);
                    buffer.WriteRawByteArray(sendBuffer.ReadToEnd());
                    var packetData = buffer.ToArray();
                    byte[] uncompressedLength;
                    if (packetData.Length >= CompressionThreshold)
                    {
                        uncompressedLength = packetData.Length.EncodeVarInt();
                        packetData = Zlib.Compress(packetData);
                    }
                    else uncompressedLength = 0.EncodeVarInt();
                    var packet = uncompressedLength.Concat(packetData);
                    var Y = packet.Length.EncodeVarInt().Concat(packet);
                    Send(Y);
                }
            }
            catch
            {
                Disconnected = true;
                Console.WriteLine("Client lost connection");
            }
        }

        private void Send(byte[] data)
        {
            tcpClient.Client.Send(data);
        }

        private byte[] Receive(int length)
        {
            var buffer = new byte[length];
            Receive(buffer, 0, buffer.Length);
            return buffer;
        }

        private void Receive(byte[] buffer, int start, int length)
        {
            if (Disconnected) return;
            try
            {
                int read = 0;
                while (read < length)
                {
                    read += tcpClient.Client.Receive(buffer, start + read, length - read, SocketFlags.None);
                }
            }
            catch
            {
                Disconnected = true;
            }
        }

        public void Close()
        {
            tcpClient.Close();
        }
    }
}
