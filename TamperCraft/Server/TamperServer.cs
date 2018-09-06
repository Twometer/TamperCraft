using Craft.Net.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TamperCraft.Proxy;

namespace TamperCraft.Server
{
    public class TamperServer
    {
        private TcpListener tcpListener;

        public List<IPacketProcessor> PacketProcessors { get; set; } = new List<IPacketProcessor>();
        public SessionToken Token { get; private set; }
        public string TargetServer { get; private set; }
        public string Motd => $"§bTwometer§r'§bs §eTamperCraft v{Program.Version}\n§9§lCURRENT TARGET: §6" + TargetServer;

        public TamperServer(SessionToken token, string targetServer)
        {
            Token = token;
            TargetServer = targetServer;
        }

        public void Start()
        {
            tcpListener = new TcpListener(IPAddress.Any, 25565);
            tcpListener.Start();
            var thread = new Thread(() =>
            {
                while (true)
                {
                    var client = tcpListener.AcceptTcpClient();
                    var tamperClient = new TamperClient(this, client);
                    Console.WriteLine("New client connected");
                }
            });
            thread.Start();
        }

    }
}
