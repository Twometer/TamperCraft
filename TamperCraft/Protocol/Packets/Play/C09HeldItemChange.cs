using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class C09HeldItemChange : IPacket
    {
        public int slotId;

        public C09HeldItemChange(int slotId)
        {
            this.slotId = slotId;
        }

        public int GetId()
        {
            return 0x09;
        }

        public void Handle(NetHandler netHandler)
        {
            throw new NotImplementedException();
        }

        public void Receive(PacketBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void Send(PacketBuffer buffer)
        {
            buffer.WriteShort((short)slotId);
        }
    }
}
