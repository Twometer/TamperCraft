using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamperCraft.Proxy;

namespace TamperCraft.Processors
{
    // Schritt 1: Wir legen unseren PacketProcessor an, der die Velocity packets schlucken wird
    public class AntiVelocityProcessor : IPacketProcessor
    {
        public void ProcessClientPacket(PacketEvent packetEvent)
        {
            
        }

        public void ProcessServerPacket(PacketEvent packetEvent)
        {
            // Schritt 2: Wir wissen, dass das Velocity Packet vom Server kommt, also gehen wir in die
            // Methode "ProcessServerPacket"
            // Schritt 3: Wir brauchen nun die Id des Velocity Packets. Dazu haben wir die Protocol Spec.
            // Also, die Id ist 0x12. Dann prüfen wir jetzt nach dem Packet 0x12
            if(packetEvent.Id == 0x12)
            {
                // Schritt 4: jetzt müssen wir das Packet "schlucken". Dazu müssen wir das PacketEvent abbrechen.
                packetEvent.Cancelled = true;
            }
        }
    }
}
