using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class C04PlayerPosition : IPacket
    {
        public double x;
        public double y;
        public double z;
        public bool onGround;

        public C04PlayerPosition(double x, double y, double z, bool onGround)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.onGround = onGround;
        }

        public int GetId()
        {
            return 0x04;
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
            buffer.WriteDouble(x);
            buffer.WriteDouble(y);
            buffer.WriteDouble(z);
            buffer.WriteBool(onGround);
        }
    }
}
