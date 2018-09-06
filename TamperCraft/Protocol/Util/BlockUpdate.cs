using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Util
{
    public class BlockUpdate
    {
        public int relativeX;
        public int relativeZ;
        public int y;
        public int newBlockId;

        public BlockUpdate(int relX, int y, int relZ, int newId)
        {
            this.relativeX = relX;
            this.y = y;
            this.relativeZ = relZ;
            this.newBlockId = newId;
        }
    }
}
