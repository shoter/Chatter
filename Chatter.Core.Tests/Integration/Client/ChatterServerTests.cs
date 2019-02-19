using Chatter.Core.Data;
using Chatter.Core.Server;
using NLog;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Xunit;

namespace Chatter.Core.Tests.Integration.Client
{
    public class ChatterServerTests
    {
        ChatterServer server;
        
        public ChatterServerTests()
        {
            server = new ChatterServer(new PacketReader(), new PacketWriter(), new NullLogger(new LogFactory()));
            server.Start(IPAddress.Parse("127.0.0.1"), 9000);
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
            Packet packet = SendPacket(new AskForPeoplePacket());
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
                return pr.ReadPacket(s);
            }
        }


    }
}
