using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Craft.Net.Auth
{
    [Serializable]
    public class SessionStore
    {
        public string lastLogin = "";
        private List<SessionToken> storedTokens = new List<SessionToken>();
        

        public void Store(string login, SessionToken token)
        {
            SessionToken existing = Receive(login);
            if(existing != null)
            {
                storedTokens.Remove(existing);
            }
            token.Login = login;
            storedTokens.Add(token);
        }

        public SessionToken Receive(string login)
        {
            if (login == null)
                return null;
            foreach(SessionToken token in storedTokens)
            {
                if(token.Login == login)
                {
                    return token;
                }
            }
            return null;
        }

        public static SessionStore Load(string path)
        {
            if (!File.Exists(Path.Combine(path, "sessionstore.xml")))
                return new SessionStore();
            using (XmlReader xmlReader = XmlReader.Create(Path.Combine(path, "sessionstore.xml")))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SessionStore));
                return (SessionStore)serializer.Deserialize(xmlReader);
            }
        }

        public void Save(string path)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SessionStore));
            XmlTextWriter writer = new XmlTextWriter(Path.Combine(path, "sessionstore.xml"), Encoding.Unicode)
            {
                Formatting = Formatting.Indented,
                Indentation = 4
            };
            writer.Close();
        }
    }
}
