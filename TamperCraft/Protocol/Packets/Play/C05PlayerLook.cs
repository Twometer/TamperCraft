using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class C05PlayerLook : IPacket
    {
        public float Yaw;
        public float Pitch;
        public bool OnGround;

        public C05PlayerLook(float yaw, float pitch)
        {
            Yaw = yaw;
            Pitch = pitch;
        }

        public int GetId()
        {
            return 0x05;
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
            buffer.WriteFloat(Yaw - 180);
            buffer.WriteFloat(-Pitch);
            buffer.WriteBool(OnGround);
        }
    }
}
