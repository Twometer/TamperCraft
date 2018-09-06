using Craft.Net;
using Craft.Net.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamperCraft.Protocol.Packets.Status
{
    public class SC01StatusPing : IPacket
    {
        public long Timestamp;

        public int GetId()
        {
            return 0x01;
        }

        public void Handle(NetHandler netHandler)
        {
            throw new NotImplementedException();
        }

        public void Receive(PacketBuffer buffer)
        {
            Timestamp = buffer.ReadLong();
        }

        public void Send(PacketBuffer buffer)
        {
            buffer.WriteLong(Timestamp);
        }
    }
}
