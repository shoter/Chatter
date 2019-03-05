using Chatter.Common;
using Chatter.Core.Data;
using Chatter.Core.Server.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chatter.Core.Server
{
    public class ChatterServer : Disposable
    {
        public List<ConnectedClient> Clients { get; } = new List<ConnectedClient>();
        public List<Packet> PacketsToSendToConnected { get; set; } = new List<Packet>();
        public bool IsRunning { get; private set; } = false;
        public uint Port { get; private set; }
        public IPAddress Address { get; private set; }

        private CancellationTokenSource mainLoopTokenSource = null;
        private IPacketReader PacketReader { get; }
        private IPacketWriter PacketWriter { get; }
        private ILogger Logger { get; }

        public ChatterServer(IPacketReader packetReader, IPacketWriter writer, ILogger logger)
        {
            this.PacketWriter = writer;
            this.PacketReader = packetReader;
            this.Logger = logger;
        }

        public void Start(IPAddress address, uint port)
        {
            if (port > 65535)
                throw new ArgumentException("Port too big!");

            this.Address = address;
            this.Port = port;

            if (mainLoopTokenSource != null)
                throw new ChatterServerException("Loop is already running!");


            mainLoopTokenSource = new CancellationTokenSource();

            Thread thread = new Thread(new ParameterizedThreadStart((_) =>
            {
                mainLoop(mainLoopTokenSource.Token);
            }));

            thread.Start();
        }

        private void mainLoop(CancellationToken token)
        {
            TcpListener listener = new TcpListener(Address, (int)Port);
            listener.Start();
            IsRunning = true;

            Task<Socket> socketTask = null;
            List<Packet> packetsToSend = new List<Packet>();

           
            while(token.IsCancellationRequested == false)
            {
                List<Packet> futurePacketsToSend = new List<Packet>();
                // try to get new connection

                if (socketTask?.IsCompleted ?? false)
                {
                    if (socketTask.IsCompletedSuccessfully)
                    {
                        // process socket
                        try
                        {
                            processNewSocket(socketTask);
                        }
                        catch (Exception e)
                        {
                            Logger.Log(LogLevel.Error, e);
                        }
                    }

                    socketTask = null;
                }
                else if (socketTask == null)
                {
                    socketTask = listener.AcceptSocketAsync();
                }

                removeDisconnectedClients();

                packetsToSend = processExistingSockets(packetsToSend);
            }
            listener.Stop();
            IsRunning = false;
        }

        private List<Packet> processExistingSockets(List<Packet> packetsToSend)
        {
            List<Packet> newPacketsToSend = new List<Packet>();
            foreach (var client in Clients)
            {
                using (var stream = new NetworkStream(client.Socket, false))
                {
                    //receive data
                    while (stream.DataAvailable)
                    {
                        var packet = PacketReader.ReadPacket(stream);
                        switch (packet)
                        {
                            case SendMessagePacket msgPacket:
                                {
                                    newPacketsToSend.Add(msgPacket);
                                    break;
                                }
                            case AskForPeoplePacket ask:
                                {
                                    newPacketsToSend.Add(new PeopleListPacket(Clients.Select(c => c.Username).ToList()));
                                    break;
                                }
                            default:
                                {
                                    SendMessage(stream, new ErrorPacket("Packet not handled!"));
                                    break;
                                }
                        }
                    }

                    //send Data

                    foreach (var packet in packetsToSend)
                    {
                        SendMessage(stream, packet);
                    }
                }
            }

            return newPacketsToSend;
        }

        private void processNewSocket(Task<Socket> socketTask)
        {
            Socket socket = socketTask.Result;
            using (var stream = new NetworkStream(socket, false))
            {
                var packet = PacketReader.ReadPacket(stream);

                switch (packet)
                {
                    case ConnectPacket connectPacket:
                        {
                            if (Clients.Any(c => c.Username == connectPacket.Username))
                            {
                                SendMessage(stream, new ConnectFailed("Username is taken!"));
                            }
                            else
                            {
                                Clients.Add(new ConnectedClient(socket, connectPacket.Username));
                                SendMessage(stream, new ConnectSuccessfullPacket());
                            }

                            break;
                        }
                    default:
                        {
                            SendMessage(stream, new ErrorPacket("Wrong packet type!"));
                            break;
                        }
                }
            }
        }

        private void removeDisconnectedClients()
        {
            for (int i = 0; i < Clients.Count;)
            {
                if (Clients[i].Socket.Connected == false)
                {
                    PacketsToSendToConnected.Add(new DisconnectPacket(Clients[i].Username));
                    Clients.RemoveAt(i);
                }
                else
                    ++i;
            }
        }

        private void SendMessage(NetworkStream s, Packet p)
        {
            byte[] bytes = PacketWriter.CreatePacket(p);
            s.Write(bytes, 0, bytes.Length);
            s.Flush();
        }

        public void Stop()
        {
            mainLoopTokenSource.Cancel();
            while (IsRunning)
            {
                Thread.Sleep(10);
            }
        }

        protected override void FreeUnmanagedObjects()
        {
            Stop();
        }



    }
}
