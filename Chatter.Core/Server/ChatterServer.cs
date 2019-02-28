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

            while (token.IsCancellationRequested == false)
            {
                using (Socket socket = listener.AcceptSocket())
                {
                    if (socket.RemoteEndPoint is IPEndPoint == false)
                        continue;

                    IPEndPoint remoteIp = socket.RemoteEndPoint as IPEndPoint;

                    string msg = string.Empty;

                    using (NetworkStream s = new NetworkStream(socket))
                    {
                        try
                        {
                            Packet packet = PacketReader.ReadPacket(s);

                            switch (packet)
                            {
                                case SendMessagePacket sendMessagePacket:
                                case AskForPeoplePacket askPacket:
                                    {
                                        // EndPoint endPoint = 
                                        // authorize 
                                        break;
                                    }
                            }

                            switch (packet)
                            {
                                case SendMessagePacket sendMessagePacket:
                                    {
                                        // Send message to everybody connected.
                                        break;
                                    }
                                case ConnectPacket connectPacket:
                                    {
                                        if (Clients.Any(c => c.Username == connectPacket.Username))
                                        {
                                            SendMessage(s, new ConnectFailed("User with this name is already connected!"));

                                        }
                                        else
                                        {
                                            SendMessage(s, new ConnectSuccessfullPacket());
                                            this.Clients.Add(new ConnectedClient()
                                            {
                                                Username = connectPacket.Username
                                            });
                                        }
                                        break;
                                    }
                            }
                        }
                        catch (Exception e)
                        {
                            Logger.Log(LogLevel.Error, e, "During reading a packet");
                        }
                    }
                }
            }
        }

        private void SendMessage(NetworkStream s, Packet p)
        {
            byte[] bytes = PacketWriter.CreatePacket(p);
            s.Write(bytes, 0, bytes.Length);
        }

        public void Stop()
        {
            mainLoopTokenSource.Cancel();
        }

        protected override void FreeManagedObjects()
        {
            mainLoopTokenSource.Cancel();
        }

    }
}
