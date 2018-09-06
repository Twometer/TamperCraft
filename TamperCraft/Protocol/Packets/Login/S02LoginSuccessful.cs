using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Login
{
    public class S02LoginSuccessful : IPacket
    {
        public string UUID;
        public string Username;

        public S02LoginSuccessful()
        {
            
        }

        public S02LoginSuccessful(string uUID, string username)
        {
            UUID = uUID;
            Username = username;
        }

        public int GetId()
        {
            return 0x02;
        }

        public void Handle(NetHandler netHandler)
        {
            netHandler.HandleS02LoginSuccesful(this);
        }

        public void Receive(PacketBuffer buffer)
        {
            UUID = buffer.ReadString();
            Username = buffer.ReadString();
        }

        public void Send(PacketBuffer buffer)
        {
            buffer.WriteString(UUID);
            buffer.WriteString(Username);
        }
    }
}
