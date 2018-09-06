using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets
{
    public interface IPacket
    {
        void Send(PacketBuffer buffer);
        void Receive(PacketBuffer buffer);
        void Handle(NetHandler netHandler);
        int GetId();
    }
}
