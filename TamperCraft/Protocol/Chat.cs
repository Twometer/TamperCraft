using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamperCraft.Protocol
{
    public class Chat
    {
        public string text;

        public Chat(string text)
        {
            this.text = text;
        }

        public static string BuildMessage(string msg)
        {
            return JsonConvert.SerializeObject(new Chat(msg));
        }
    }
}
