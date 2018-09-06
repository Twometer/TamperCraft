using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamperCraft.Protocol.Packets.Status
{
    public class StatusResponse
    {
        public VersionInfo Version;
        public PlayerInfo Players;
        public DescriptionInfo Description;

        public StatusResponse(VersionInfo version, PlayerInfo players, DescriptionInfo description)
        {
            Version = version;
            Players = players;
            Description = description;
        }

        public class VersionInfo
        {
            public string Name;
            public int Protocol;

            public VersionInfo(string name, int protocol)
            {
                Name = name;
                Protocol = protocol;
            }
        }

        public class PlayerInfo
        {
            public int Max;
            public int Online;
            public IList<Player> Sample;

            public PlayerInfo(int max, int online)
            {
                Max = max;
                Online = online;
                Sample = new List<Player>();
            }
        }

        public class Player
        {
            public string Name;
            public string Id;

            public Player(string name, string id)
            {
                Name = name;
                Id = id;
            }
        }

        public class DescriptionInfo
        {
            public string Text;

            public DescriptionInfo(string text)
            {
                Text = text;
            }
        }
    }
}
