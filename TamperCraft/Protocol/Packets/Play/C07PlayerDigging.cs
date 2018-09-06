using Craft.Net.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class C07PlayerDigging : IPacket
    {
        public enum Action
        {
            START_DESTROY_BLOCK,
            ABORT_DESTROY_BLOCK,
            STOP_DESTROY_BLOCK,
            DROP_ALL_ITEMS,
            DROP_ITEM,
            RELEASE_USE_ITEM
        }

        public Action status;
        public BlockPos position;
        public byte facing;

        public C07PlayerDigging(Action status, BlockPos position, byte facing)
        {
            this.status = status;
            this.position = position;
            this.facing = facing;
        }

        public int GetId()
        {
            return 0x07;
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
            buffer.WriteVarInt((int)status);
            buffer.WriteULong((ulong)position.ToLong());
            buffer.WriteByte(facing);
        }
    }
}
