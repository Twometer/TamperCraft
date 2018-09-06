using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamperCraft.Proxy;

namespace TamperCraft.Processors
{
    public class FlightProcessor : IPacketProcessor
    {
        public static bool IsEnabled { get; set; }

        public void ProcessClientPacket(PacketEvent packetEvent)
        {
            if(IsEnabled && packetEvent.Id == 0x13)
            {
                packetEvent.Cancelled = true;
            }
        }

        public void ProcessServerPacket(PacketEvent packetEvent)
        {
            if(packetEvent.Id == 0x39)
            {

            }
        }
    }
}
