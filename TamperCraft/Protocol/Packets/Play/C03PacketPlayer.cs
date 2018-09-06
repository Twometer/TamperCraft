using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class C03PacketPlayer : IPacket
    {
        public bool OnGround;

        public C03PacketPlayer(bool onGround)
        {
            OnGround = onGround;
        }

        public int GetId()
        {
            return 0x03;
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
            buffer.WriteBool(OnGround);
        }
    }
}
