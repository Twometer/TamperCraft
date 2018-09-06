using Craft.Net.Packets.Play;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Client.Inventory
{
    public class Inventory
    {
        public ItemStack[] slots = new ItemStack[40];
        public int currentItem;

        public ItemStack GetCurrentStack()
        {
            return slots[currentItem];
        }

        public void UpdateSlot(int slot, ItemStack stack)
        {
            if (slot >= 0)
            {
                if (slot >= 36)
                    slot -= 36;
                slots[slot] = stack;
            }
        }

        public void SetCurrentStack(int id)
        {
            if (currentItem != id)
            {
                currentItem = id;
                //CraftGame.GetCraft().NetworkManager.SendPacket(new C09HeldItemChange(id));
            }
        }

        public ItemStack[] GetHotbar()
        {
            ItemStack[] itst = new ItemStack[9];
            Array.Copy(slots, itst, 9);
            return itst;
        }

    }
}
