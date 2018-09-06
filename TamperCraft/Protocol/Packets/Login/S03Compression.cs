using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Login
{
    public class S03Compression : IPacket
    {
        public int Threshold;

        public S03Compression()
        {
            
        }

        public S03Compression(int threshold)
        {
            Threshold = threshold;
        }

        public int GetId()
        {
            return 0x03;
        }

        public void Handle(NetHandler netHandler)
        {
            netHandler.HandleS03Compression(this);
        }

        public void Receive(PacketBuffer buffer)
        {
            Threshold = buffer.ReadVarInt();
        }

        public void Send(PacketBuffer buffer)
        {
            buffer.WriteVarInt(Threshold);
        }
    }
}
