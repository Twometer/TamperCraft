using Craft.Net.Packets.Play;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamperCraft.Proxy;

namespace TamperCraft.Processors.Command
{
    [CommandData("help", "Displays a list of commands")]
    public class HelpCommand : ICommand
    {
        public void Execute(CommandManager manager, PacketEvent e, string[] args)
        {
            manager.Processor.SendChat(e, "[§9TamperCraft§r] Help", S02ChatMessage.ChatPosition.Chat);
            foreach(var cmd in manager.Commands)
            {
                CommandData data = null;
                foreach (var attr in cmd.GetType().GetCustomAttributes(false))
                    if (attr is CommandData) data = (CommandData)attr;
                if(data != null) manager.Processor.SendChat(e, $"[§9TamperCraft§r] {data.Name}: {data.Description}", S02ChatMessage.ChatPosition.Chat);
            }
        }
    }
}
