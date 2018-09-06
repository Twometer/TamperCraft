using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class C06PlayerPosLook : IPacket
    {
        public double X;
        public double Y;
        public double Z;
        public float Yaw;
        public float Pitch;
        public bool OnGround;

        public C06PlayerPosLook(double x, double y, double z, float yaw, float pitch, bool onGround)
        {
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            OnGround = onGround;
        }

        public int GetId()
        {
            return 0x06;
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
            buffer.WriteDouble(X);
            buffer.WriteDouble(Y);
            buffer.WriteDouble(Z);

            buffer.WriteFloat(Yaw-180);
            buffer.WriteFloat(-Pitch);

            buffer.WriteBool(OnGround);
        }
    }
}
