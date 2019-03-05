using Chatter.Core.Data;
using Chatter.Core.Server;
using NLog;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Xunit;

namespace Chatter.Core.Tests.Integration.Server
{
    public class ChatterServerIntTests : IDisposable
    {
        ChatterServer server;
        
        public ChatterServerIntTests()
        {
            server = new ChatterServer(new PacketReader(), new PacketWriter(), new NullLogger(new LogFactory()));
            server.Start(IPAddress.Parse("127.0.0.1"), 9000);
            int i = 0;
            while(server.IsRunning == false)
            {
                Thread.Sleep(100);
                if(i++ > 10)
                {
                    throw new Exception("Server did not start!");
                }
            }
        }

        [Fact]
        public void OnFirstConnect_ShouldReceiveConnectSuccessfull()
        {
            Packet packet = SendPacket(new ConnectPacket("shoter"));
            Assert.True(packet is ConnectSuccessfullPacket);
        }

        [Fact]
        public void OnAskPeople_WithoutConnecting_Error()
        {
            Packet packet = SendPacket(new AskForPeoplePacket("Randomusername"));
            Assert.True(packet is ErrorPacket);
        }

        [Fact]
        public void OnSendMessage_WithoutConnect_ShouldReturnError()
        {
            Packet packet = SendPacket(new SendMessagePacket("Message", "shoter", DateTime.Now));
            Assert.True(packet is ErrorPacket);
        }

        [Fact]
        public void OnSecondConnect_ShouldReceiveError_CannotConnectTwoTimesInRow()
        {
            Packet packet = SendPacket(new ConnectPacket("asd"));
            packet = SendPacket(new ConnectPacket("asd"));

            Assert.True(packet is ConnectFailed);
        }

        private Packet SendPacket(Packet packet)
        {
            PacketWriter pw = new PacketWriter();
            PacketReader pr = new PacketReader();

            TcpClient client = new TcpClient("127.0.0.1", 9000);
            using (NetworkStream s = client.GetStream())
            {
                var bytes = pw.CreatePacket(packet);
                s.Write(bytes, 0, bytes.Length);
                var returnPacket = pr.ReadPacket(s);
                return returnPacket;
            }
        }

        public void Dispose()
        {
            server.Dispose();
        }
    }
}
