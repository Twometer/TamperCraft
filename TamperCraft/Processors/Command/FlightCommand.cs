using Craft.Net.Packets.Play;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamperCraft.Proxy;

namespace TamperCraft.Processors.Command
{
    [CommandData("flight", "Toggles or configures the flight module")]
    public class FlightCommand : ICommand
    {
        public void Execute(CommandManager manager, PacketEvent e, string[] args)
        {
            FlightProcessor.IsEnabled = !FlightProcessor.IsEnabled;
            manager.Processor.SendChat(e, $"Flying {(FlightProcessor.IsEnabled ? "enabled" : "disabled")}", S02ChatMessage.ChatPosition.Chat);
            //e.Client.ProxyConnection
        }
    }
}
