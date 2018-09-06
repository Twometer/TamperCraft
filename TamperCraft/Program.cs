using Craft.Net.Auth;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using TamperCraft.Login;
using TamperCraft.Processors;
using TamperCraft.Server;

namespace TamperCraft
{
    internal class Program
    {

        public static string FullVersion => Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static string Version => FullVersion.Split('.')[0] + "." + FullVersion.Split('.')[1];

        public static void Main(string[] args)
        {
            Console.WriteLine($"== TamperCraft v{Version} ==");
            Console.WriteLine();

            var configFile = new FileInfo("config.txt");
            var reader = new StreamReader(configFile.FullName);

            var targetServer = reader.ReadLine();
            var username = reader.ReadLine();
            var password = reader.ReadLine();

            var credentialManager = new CredentialManager();
            var loginResult = credentialManager.LogIn(username, password, out SessionToken token);
            if (loginResult != LoginResult.Success)
            {
                Quit("Error: Could not log in with the specified credentials (" + loginResult + ")");
                return;
            }
            else
            {
                Console.WriteLine("Mojang authentication successful");
            }
            var server = new TamperServer(token, targetServer);
            //server.PacketProcessors.Add(new AntiVelocityProcessor());
            server.PacketProcessors.Add(new FlightProcessor());
            server.PacketProcessors.Add(new ChatProcessor());
            server.Start();

            Console.WriteLine("Server started");
            while (true) Thread.Sleep(100000);
        }

        private static void Quit(string msg)
        {
            Console.WriteLine(msg);
            Console.ReadKey();
        }
    }
}
