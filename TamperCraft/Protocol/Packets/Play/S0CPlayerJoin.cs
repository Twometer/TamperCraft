using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class S0CPlayerJoin : IPacket
    {
        public int playerId;
        public string uuid;
        public int x;
        public int y;
        public int z;

        public int GetId()
        {
            return 0x0C;
        }

        public void Handle(NetHandler netHandler)
        {
            netHandler.HandleS0CPlayerJoin(this);
        }

        public void Receive(PacketBuffer buffer)
        {
            playerId = buffer.ReadVarInt();
            buffer.ReadULong(); buffer.ReadULong();
            x = buffer.ReadInt() >> 5;
            y = buffer.ReadInt() >> 5;
            z = buffer.ReadInt() >> 5;
            buffer.ReadByte(); buffer.ReadByte();
        }

        public void Send(PacketBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
