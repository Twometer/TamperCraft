using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class S40Disconnect : IPacket
    {
        public string jsonReason;

        public int GetId()
        {
            return 0x40;
        }

        public void Handle(NetHandler netHandler)
        {
            netHandler.HandleS40Disconnect(this);
        }

        public void Receive(PacketBuffer buffer)
        {
            this.jsonReason = buffer.ReadString();
        }

        public void Send(PacketBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
