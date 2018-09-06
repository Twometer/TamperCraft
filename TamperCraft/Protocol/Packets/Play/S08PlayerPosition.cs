using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class S08PlayerPosition : IPacket
    {
        public double x;
        public double y;
        public double z;

        public float yaw;
        public float pitch;

        public int GetId()
        {
            return 0x08;
        }

        public void Handle(NetHandler netHandler)
        {
            netHandler.HandleS08PlayerPosition(this);
        }

        public void Receive(PacketBuffer buffer)
        {
            this.x = buffer.ReadDouble();
            this.y = buffer.ReadDouble();
            this.z = buffer.ReadDouble();

            this.yaw = buffer.ReadFloat();
            this.pitch = buffer.ReadFloat();
        }

        public void Send(PacketBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
