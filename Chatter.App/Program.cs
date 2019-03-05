using Chatter.Core;
using Chatter.Core.Client;
using Chatter.Core.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Chatter.App
{
    class Program
    {
        public static ConcurrentQueue<string> ToDisplay { get; set; } = new ConcurrentQueue<string>();
        public static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                string ip = args[0];
                uint port = uint.Parse(args[1]);

                Console.WriteLine($"Joining server on {ip}:{port}");

                var client = new ChatterClient(new PacketReader(), new PacketWriter());

                

                Console.WriteLine("Provide your username");
                var username = Console.ReadLine();

                client.Connect(ip, port, username);

                Console.WriteLine("Connected!");

                client.OnClientEvent += Client_OnClientEvent;

                while(client.IsConnected)
                {
                    var input = Console.ReadLine();
                    
                    switch(input)
                    {
                        case "":
                            {
                                while(ToDisplay.IsEmpty == false)
                                {
                                    if (ToDisplay.TryDequeue(out string result))
                                        Console.WriteLine(result);
                                }
                                break;
                            }
                        case "!quit":
                            {
                                client.Stop();
                                break;
                            }
                        case "!people":
                            {
                                client.AskForPeople();
                                break;
                            }
                        default:
                            {
                                client.SendMessage(input);
                                break;
                            }
                    }
                }

            }

            Console.WriteLine("Usage: [ip] [port]");
        }

        private static void Client_OnClientEvent(object sender, Core.Client.Events.ClientEventEventArgs e)
        {
            switch(e.ClientEvent)
            {
                case MessageReceivedEvent msg:
                    {
                        ToDisplay.Enqueue($"{msg.Username} : {msg.Message}");
                        break;
                    }
                case UserDisconnectedEvent disc:
                    {
                        ToDisplay.Enqueue($"{disc.Username} disconnected!");
                        break;
                    }
                case UserListEvent ul:
                    {
                        ToDisplay.Enqueue("User list: ");
                        ToDisplay.Enqueue(string.Join(Environment.NewLine, ul.Users));
                        break;
                    }
            }
        }
    }
}
