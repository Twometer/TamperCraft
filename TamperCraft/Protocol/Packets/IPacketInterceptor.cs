using Craft.Net.Packets;

namespace TamperCraft.Protocol.Packets
{
    public interface IPacketInterceptor
    {
        void OnClientPacket(int id, PacketBuffer buf);
        void OnServerPacket(int id, PacketBuffer buf);
    }
}
