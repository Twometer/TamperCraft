using Craft.Net.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamperCraft.Login
{
    [Serializable]
    public class MojangCredential
    {
        public MojangCredential(string email, byte[] passwordHash, SessionToken lastToken)
        {
            Email = email;
            PasswordHash = passwordHash;
            LastToken = lastToken;
        }

        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public SessionToken LastToken { get; set; }
    }
}
