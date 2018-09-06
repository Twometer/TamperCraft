using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class S06HealthChanged : IPacket
    {
        public float health;

        public int GetId()
        {
            return 0x06;
        }

        public void Handle(NetHandler netHandler)
        {
            netHandler.HandleS06HealthChanged(this);
        }

        public void Receive(PacketBuffer buffer)
        {
            health = buffer.ReadFloat();
        }

        public void Send(PacketBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
