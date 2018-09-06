using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamperCraft.Proxy;

namespace TamperCraft.Processors.Command
{
    public interface ICommand
    {
        void Execute(CommandManager manager, PacketEvent e, string[] args);
    }
}
