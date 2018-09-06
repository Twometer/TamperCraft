using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Login
{
    public class S01EncryptionRequest : IPacket
    {

        public string serverId;
        public byte[] serverKey;
        public byte[] token;

        public int GetId()
        {
            return 0x01;
        }

        public void Handle(NetHandler netHandler)
        {
            netHandler.HandleS01EncryptionRequest(this);
        }

        public void Receive(PacketBuffer buffer)
        {
            serverId = buffer.ReadString();
            serverKey = buffer.ReadByteArray();
            token = buffer.ReadByteArray();
        }

        public void Send(PacketBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
