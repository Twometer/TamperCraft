using Craft.Net.Packets;
using Craft.Net.Packets.Play;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamperCraft.Processors.Command;
using TamperCraft.Protocol;
using TamperCraft.Proxy;
using static Craft.Net.Packets.Play.S02ChatMessage;

namespace TamperCraft.Processors
{
    public class ChatProcessor : IPacketProcessor
    {
        private CommandManager commandManager;

        public ChatProcessor()
        {
            commandManager = new CommandManager(this);
        }

        public void ProcessClientPacket(PacketEvent packetEvent)
        {
            if (packetEvent.Id != 0x01) return;
            var str = packetEvent.Buffer.ReadString();
            if (str.StartsWith("."))
            {
                commandManager.HandleCommand(packetEvent, str);
                packetEvent.Cancelled = true;
                return;
            }
            var buf = new PacketBuffer();
            buf.WriteString(str);
            buf.Reset();
            packetEvent.Buffer = buf;
        }

        public void ProcessServerPacket(PacketEvent packetEvent)
        {
            
        }

        public void SendChat(PacketEvent e, string message, ChatPosition pos)
        {
            e.Client.ProxyConnection.SendPacket(new S02ChatMessage(Chat.BuildMessage(message), pos));
        }

    }
}
