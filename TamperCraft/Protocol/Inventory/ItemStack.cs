using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Client.Inventory
{
    public class ItemStack
    {
        public int id;
        public int count;
        public int meta;

        public ItemStack(int id, int count, int meta)
        {
            this.id = id;
            this.count = count;
            this.meta = meta;
        }
    }
}
