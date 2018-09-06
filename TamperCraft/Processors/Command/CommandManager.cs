using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamperCraft.Proxy;

namespace TamperCraft.Processors.Command
{
    public class CommandManager
    {
        public ChatProcessor Processor { get; set; }

        public ICommand[] Commands { get; } = new ICommand[]
        {
            new HelpCommand(),
            new FlightCommand()
        };

        public CommandManager(ChatProcessor processor)
        {
            Processor = processor;
        }

        public void HandleCommand(PacketEvent e, string input)
        {
            input = input.Substring(1);
            var arr = input.Split(' ');
            var name = arr[0];

            foreach (var cmd in Commands)
            {
                CommandData data = null;
                foreach (var attr in cmd.GetType().GetCustomAttributes(false))
                {
                    if (attr is CommandData) data = (CommandData)attr;
                }
                if (data?.Name == name)
                {
                    if (arr.Length == 1) cmd.Execute(this, e, new string[] { });
                    else
                    {
                        var args = new string[arr.Length - 1];
                        Array.Copy(arr, 1, args, 0, args.Length);
                        cmd.Execute(this, e, args);
                    }
                }
            }
        }
    }
}
