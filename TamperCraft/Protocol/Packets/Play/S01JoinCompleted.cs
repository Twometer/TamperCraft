using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class S01JoinCompleted : IPacket
    {
        public int playerEid;
        public int gamemode;
        public bool hardcore;
        public int dimension;
        public int difficulty;
        public int maxPlayers;
        public string LevelType;
        public bool reducedInfo;

        public int GetId()
        {
            return 0x01;
        }

        public void Handle(NetHandler netHandler)
        {
            netHandler.HandleS01JoinCompleted(this);
        }

        public void Receive(PacketBuffer buffer)
        {
            playerEid = buffer.ReadInt();
            int i = buffer.ReadByte();
            hardcore = (i & 8) == 8;
            gamemode = i & -9;
            dimension = buffer.ReadSByte();
            difficulty = buffer.ReadByte();
            maxPlayers = buffer.ReadByte();
            LevelType = buffer.ReadString();
            reducedInfo = buffer.ReadBool();
        }

        public void Send(PacketBuffer buffer)
        {
            throw new NotImplementedException();
        }
    }
}
