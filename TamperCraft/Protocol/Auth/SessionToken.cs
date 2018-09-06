using System;

namespace Craft.Net.Auth
{
    [Serializable]
    public class SessionToken
    {
        

        public string ID { get; set; }
        public string PlayerName { get; set; }
        public string PlayerID { get; set; }
        public string ClientID { get; set; }
        public string Login { get; set; }

        public SessionToken()
        {
            ID = String.Empty;
            PlayerName = String.Empty;
            PlayerID = String.Empty;
            ClientID = String.Empty;
        }

        public SessionToken(string iD, string playerName, string playerID, string clientID)
        {
            ID = iD;
            PlayerName = playerName;
            PlayerID = playerID;
            ClientID = clientID;
        }
    }
}
