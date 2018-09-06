using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class S00KeepAlive : IPacket
    {
        public int id;

        public int GetId()
        {
            return 0x00;
        }

        public void Handle(NetHandler netHandler)
        {
            netHandler.HandleS00KeepAlive(this);
        }

        public void Receive(PacketBuffer buffer)
        {
            id = buffer.ReadVarInt();
        }

        public void Send(PacketBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
