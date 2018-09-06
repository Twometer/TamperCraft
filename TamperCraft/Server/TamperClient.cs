using Craft.Net;
using Craft.Net.Packets;
using Craft.Net.Packets.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TamperCraft.Protocol;
using TamperCraft.Protocol.Packets;
using TamperCraft.Protocol.Packets.Status;
using TamperCraft.Proxy;
using TamperCraft.Util;

namespace TamperCraft.Server
{
    public class TamperClient : IPacketInterceptor
    {
        public MinecraftConnection TargetConnection { get; private set; }
        public ProxyConnection ProxyConnection { get; private set; }

        private ProxyState proxyState;

        private TamperServer parent;

        public TamperClient(TamperServer server, TcpClient tcpClient)
        {
            parent = server;
            ProxyConnection = new ProxyConnection(tcpClient);
            ProxyConnection.Interceptor = this;
        }

        private void Connect()
        {
            var lParams = new LoginParams()
            {
                ServerHost = parent.TargetServer,
                ServerPort = 25565,
                Username = parent.Token.PlayerName,
                UUID = parent.Token.PlayerID,
                SessionId = parent.Token.ID
            };
            TargetConnection = new MinecraftConnection();
            TargetConnection.Interceptor = this;
            if (!TargetConnection.Login(lParams))
            {
                ProxyConnection.SendPacket(new S00LoginRejected(Chat.BuildMessage("§c> TamperCraft Error <\n\n§rCould not connect to the target server.")));
                ProxyConnection.Close();
                TargetConnection.Close();
                return;
            }
            TargetConnection.WaitForConnect();
        }

        public void OnClientPacket(int id, PacketBuffer buf)
        {
            switch (proxyState)
            {
                case ProxyState.Handshake:
                    if (id == 0x00) HandleP00Handshake(buf.AsPacket<C00PacketHandshake>());
                    break;
                case ProxyState.Login:
                    if (id == 0x00) HandleP00Login(buf.AsPacket<C00PacketLogin>());
                    break;
                case ProxyState.Status:
                    if (id == 0x00) HandleP00StatusRequest();
                    else if (id == 0x01) HandleP01StatusPing(buf.AsPacket<SC01StatusPing>());
                    break;
                case ProxyState.Established:
                    foreach(var proc in parent.PacketProcessors)
                    {
                        var e = new PacketEvent(this, id, buf);
                        proc.ProcessClientPacket(e);
                        buf = e.Buffer;
                        if (e.Cancelled) return;
                    }
                    TargetConnection.SendPacket(id, buf);
                    break;
            }
        }

        public void OnServerPacket(int id, PacketBuffer buf)
        {
            foreach (var proc in parent.PacketProcessors)
            {
                var e = new PacketEvent(this, id, buf);
                proc.ProcessServerPacket(e);
                buf = e.Buffer;
                if (e.Cancelled) return;
            }
            ProxyConnection.SendPacket(id, buf);
        }

        private void HandleP00Handshake(C00PacketHandshake packet)
        {
            proxyState = (ProxyState)packet.nextState;
        }

        private void HandleP00StatusRequest()
        {
            var versionInfo = new StatusResponse.VersionInfo($"TamperCraft {Program.Version}", 47);
            var playerInfo = new StatusResponse.PlayerInfo(0, 0);
            playerInfo.Sample.Add(new StatusResponse.Player("¯\\_ツ_/¯", Guid.NewGuid().ToString()));
            var descriptionInfo = new StatusResponse.DescriptionInfo(parent.Motd);
            var response = new StatusResponse(versionInfo, playerInfo, descriptionInfo);
            ProxyConnection.SendPacket(new S00StatusResponse(response));
        }

        private void HandleP01StatusPing(SC01StatusPing packet)
        {
            ProxyConnection.SendPacket(packet);
        }

        private void HandleP00Login(C00PacketLogin packet)
        {
            Connect();
            ProxyConnection.SendPacket(new S03Compression(256));
            ProxyConnection.CompressionThreshold = 256;
            ProxyConnection.SendPacket(new S02LoginSuccessful(NormalizeUuid(parent.Token.PlayerID), parent.Token.PlayerName));
            proxyState = ProxyState.Established;
        }

        private string NormalizeUuid(string uuid)
        {
            return Guid.ParseExact(uuid, "N").ToString();
        }

    }
}
