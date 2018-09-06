using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets
{
    public class C00PacketLogin : IPacket
    {
        public string username;

        public C00PacketLogin(string username)
        {
            this.username = username;
        }

        public C00PacketLogin()
        {

        }

        public int GetId()
        {
            return 0x00;
        }

        public void Handle(NetHandler netHandler)
        {
            throw new NotImplementedException();
        }

        public void Receive(PacketBuffer buffer)
        {
            username = buffer.ReadString();
        }

        public void Send(PacketBuffer buffer)
        {
            buffer.WriteString(username);
        }
    }
}
