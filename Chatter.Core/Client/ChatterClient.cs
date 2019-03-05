using Chatter.Common;
using Chatter.Core.Client.Events;
using Chatter.Core.Client.Exceptions;
using Chatter.Core.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chatter.Core.Client
{
    public class ChatterClient : Disposable
    {
        public event EventHandler<ClientEventEventArgs> OnClientEvent;

        public bool IsConnected { get; set; } = false;

        public string Username { get; set; }

        private TcpClient Client { get; set; }

        private CancellationTokenSource mainLoopTokenSource { get; } = new CancellationTokenSource();
        private IPacketReader PacketReader { get; }
        private IPacketWriter PacketWriter { get; }

        private ConcurrentQueue<Packet> ToSendQueue { get; } = new ConcurrentQueue<Packet>();

        public ChatterClient(IPacketReader reader, IPacketWriter writer)
        {
            this.PacketReader = reader;
            this.PacketWriter = writer;
        }

        public void AskForPeople()
        {
            enque(new AskForPeoplePacket(this.Username));
        }

        public void SendMessage(string msg)
        {
            enque(new SendMessagePacket(msg, this.Username, DateTime.Now));
        }

        private void enque(Packet packet)
        {
            this.ToSendQueue.Enqueue(packet);
        }


        public void Connect(string address, uint port, string username)
        {
            if (port > 65535)
                throw new ArgumentException("port");

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("username");

            this.Username = username;

            Client = new TcpClient(address, (int)port);

            var packet = sendPacketAsync(new ConnectPacket(this.Username), Client.GetStream());

            switch (packet)
            {
                case ConnectSuccessfullPacket succ:
                    {
                        IsConnected = true;

                        break;
                    }
                case ErrorPacket e:
                    {
                        throw new Exception(e.Error);
                    }
                default:
                    {
                        throw new Exception("Wrong packet type");
                    }

            }

            ParameterizedThreadStart param = new ParameterizedThreadStart((_) =>
            {
                this.loop(this.mainLoopTokenSource.Token);
            });
            Thread thread = new Thread(param);

            thread.Start();

            //create thread. Now everything will be threaded
        }

        public void Stop()
        {
            if (IsConnected == false)
                throw new Exception("Not connected!");

            mainLoopTokenSource.Cancel();

            while (IsConnected)
            {
                Thread.Sleep(100);
            }
        }

        protected override void FreeUnmanagedObjects()
        {
            base.FreeUnmanagedObjects();
            this.Stop();
        }

        private void loop(CancellationToken token)
        {
            using (NetworkStream stream = Client.GetStream())
                while (token.IsCancellationRequested == false)
                {
                    while (stream.DataAvailable)
                    {
                        var packet = PacketReader.ReadPacket(stream);

                        switch (packet)
                        {
                            case PeopleListPacket peopleList:
                                {
                                    invokeEvent(new UserListEvent(peopleList.Usernames));
                                    break;
                                }
                            case SendMessagePacket sendMessage:
                                {
                                    invokeEvent(new MessageReceivedEvent(sendMessage.Username, sendMessage.Message));
                                    break;
                                }
                            case DisconnectPacket disconnect:
                                {
                                    invokeEvent(new UserDisconnectedEvent(disconnect.Username));
                                    break;
                                }
                            case ErrorPacket e:
                                {
                                    throw new Exception(e.Error);
                                }
                            default:
                                {
                                    throw new Exception("Not handled packet!");
                                }
                        }
                    }

                    while (this.ToSendQueue.IsEmpty == false)
                    {
                        if (this.ToSendQueue.TryDequeue(out Packet packet))
                        {
                            var bytes = PacketWriter.CreatePacket(packet);
                            stream.Write(bytes, 0, bytes.Length);
                        }
                    }
                }

            this.Client.Close();
            IsConnected = false;
        }

        private void invokeEvent(ClientEvent e)
        {
            this.OnClientEvent?.Invoke(this, new ClientEventEventArgs(e));
        }

        private Packet sendPacketAsync(Packet packet, NetworkStream s)
        {

            var bytes = PacketWriter.CreatePacket(packet);
            s.Write(bytes, 0, bytes.Length);
            var returnPacket =  PacketReader.ReadPacket(s);
            return returnPacket;

        }

    }
}