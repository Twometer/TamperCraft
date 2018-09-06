using Craft.Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Craft.Client.Inventory;

namespace Craft.Net.Packets.Play
{
    public class C08PlayerBlockPlacement : IPacket
    {
        private static BlockPos NULL_POSITION = new BlockPos(-1, -1, -1);

        private BlockPos position;
        private byte placedBlockDirection;
        private ItemStack stack;
        private float facingX, facingY, facingZ;

        public C08PlayerBlockPlacement(ItemStack itemStack)
        {
            new C08PlayerBlockPlacement(NULL_POSITION, 255, itemStack, 0.0F, 0.0F, 0.0F);
        }

        public C08PlayerBlockPlacement(BlockPos positionIn, byte placedBlockDirectionIn, ItemStack stackIn, float facingXIn, float facingYIn, float facingZIn)
        {
            this.position = positionIn;
            this.placedBlockDirection = placedBlockDirectionIn;
            this.stack = stackIn;
            this.facingX = facingXIn;
            this.facingY = facingYIn;
            this.facingZ = facingZIn;
        }

        public int GetId()
        {
            return 0x08;
        }

        public void Handle(NetHandler netHandler)
        {
            throw new NotImplementedException();
        }

        public void Receive(PacketBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public void Send(PacketBuffer buffer)
        {
            buffer.WriteULong((ulong)position.ToLong());
            buffer.WriteByte(placedBlockDirection);

            buffer.WriteShort((short)stack.id);
            buffer.WriteByte((byte)stack.count);
            buffer.WriteShort((short)stack.meta);
            buffer.WriteBool(false);

            buffer.WriteByte((byte)(facingX * 16));
            buffer.WriteByte((byte)(facingY * 16));
            buffer.WriteByte((byte)(facingZ * 16));
        }
    }
}
