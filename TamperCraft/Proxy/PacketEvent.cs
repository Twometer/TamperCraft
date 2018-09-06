using Craft.Net.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamperCraft.Server;

namespace TamperCraft.Proxy
{
    public class PacketEvent
    {
        public PacketEvent(TamperClient client, int id, PacketBuffer buffer)
        {
            Client = client;
            Id = id;
            Buffer = buffer;
        }

        public TamperClient Client { get; set; }
        public int Id { get; set; }
        public PacketBuffer Buffer { get; set; }
        public bool Cancelled { get; set; }
    }
}
