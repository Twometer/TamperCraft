using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamperCraft.Proxy
{
    public enum ProxyState
    {
        Handshake = 0,
        Status = 1,
        Login = 2,
        Established = -1
    }
}
