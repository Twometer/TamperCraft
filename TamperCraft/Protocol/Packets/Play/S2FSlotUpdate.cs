using Craft.Client.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class S2FSlotUpdate : IPacket
    {
        public int windowId;
        public int slotId;
        public ItemStack stack;

        public int GetId()
        {
            return 0x2F;
        }

        public void Handle(NetHandler netHandler)
        {
            netHandler.HandleS2FSlotUpdate(this);
        }

        public void Receive(PacketBuffer buffer)
        {
            windowId = buffer.ReadSByte();
            slotId = buffer.ReadShort();
            stack = null;

            int itemId = buffer.ReadShort();
            if(itemId > 0)
            {
                int amount = buffer.ReadByte();
                int meta = buffer.ReadShort();
                stack = new ItemStack(itemId, amount, meta);
            }
        }

        public void Send(PacketBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
