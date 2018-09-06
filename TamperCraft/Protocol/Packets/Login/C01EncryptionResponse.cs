using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Login
{
    public class C01EncryptionResponse : IPacket
    {
        public byte[] secretKey;
        public byte[] token;

        public C01EncryptionResponse(byte[] secretKey, byte[] token)
        {
            this.secretKey = secretKey;
            this.token = token;
        }

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
            throw new NotImplementedException();
        }

        public void Send(PacketBuffer buffer)
        {
            buffer.WriteByteArray(secretKey);
            buffer.WriteByteArray(token);
        }
    }
}
