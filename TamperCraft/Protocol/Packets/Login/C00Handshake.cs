using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Craft.Net.Packets
{
    public class C00PacketHandshake : IPacket
    {
        public int protocolVersion;
        public string serverAdress;
        public ushort serverPort;
        public int nextState;

        public C00PacketHandshake()
        {
        }

        public C00PacketHandshake(int protocolVersion, string serverAdress, ushort serverPort, int nextState)
        {
            this.protocolVersion = protocolVersion;
            this.serverAdress = serverAdress;
            this.serverPort = serverPort;
            this.nextState = nextState;
        }

        public int GetId()
        {
            return 0x00;
        }

        public void Handle(NetHandler netHandler)
        {
            throw new NotImplementedException();
        }

        public void Receive(PacketBuffer buffer)
        {
            protocolVersion = buffer.ReadVarInt();
            serverAdress = buffer.ReadString();
            serverPort = buffer.ReadUShort();
            nextState = buffer.ReadVarInt();
        }

        public void Send(PacketBuffer buffer)
        {
            buffer.WriteVarInt(protocolVersion);
            buffer.WriteString(serverAdress);
            buffer.WriteUShort(serverPort);
            buffer.WriteVarInt(nextState);
        }
    }
}
