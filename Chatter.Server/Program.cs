using Chatter.Core;
using Chatter.Core.Server;
using NLog;
using System;
using System.Net;
using System.Threading;

namespace Chatter.Server
{
    class Program
    {
        //First arg - whatever
        static void Main(string[] args)
        {
            if(args.Length == 2)
            {
                string ip = args[0];
                uint port = uint.Parse(args[1]);

                Console.WriteLine($"Hosting server on {ip}:{port}");

                var server = new ChatterServer(new PacketReader(), new PacketWriter(), LogManager.GetCurrentClassLogger());

                server.Start(IPAddress.Parse(ip), port);

                while (server.IsRunning == false) Thread.Sleep(100);

                while(server.IsRunning)
                {
                    Thread.Sleep(100);
                }
            }
            else

            Console.WriteLine("Usage: [ip] [port]");
        }
    }
}
