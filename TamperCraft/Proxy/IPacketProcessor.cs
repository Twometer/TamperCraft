using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamperCraft.Proxy
{
    public interface IPacketProcessor
    {
        void ProcessServerPacket(PacketEvent packetEvent);
        void ProcessClientPacket(PacketEvent packetEvent);
    }
}
