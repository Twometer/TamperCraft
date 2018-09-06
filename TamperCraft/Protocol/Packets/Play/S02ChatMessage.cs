using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets.Play
{
    public class S02ChatMessage : IPacket
    {
        public string JsonMessage;
        public ChatPosition Position;

        public S02ChatMessage()
        {

        }

        public S02ChatMessage(string jsonMessage, ChatPosition position)
        {
            JsonMessage = jsonMessage;
            Position = position;
        }

        public int GetId()
        {
            return 0x02;
        }

        public void Handle(NetHandler netHandler)
        {
            netHandler.HandleS02ChatMessage(this);
        }

        public void Receive(PacketBuffer buffer)
        {
            JsonMessage = buffer.ReadString();
        }

        public void Send(PacketBuffer buffer)
        {
            buffer.WriteString(JsonMessage);
            buffer.WriteByte((byte)Position);
        }

        public enum ChatPosition
        {
            Chat = 0,
            System = 1,
            Hotbar = 2
        }
    }
}
