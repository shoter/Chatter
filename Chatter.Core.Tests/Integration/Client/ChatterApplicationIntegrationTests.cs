using Chatter.Core.Client;
using Chatter.Core.Client.Events;
using Chatter.Core.Client.Exceptions;
using Chatter.Core.Server;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Chatter.Core.Tests.Integration
{
    public class ChatterApplicationIntegrationTests
    {
        [Fact]
        public void Disconnect_WithoutConnectShouldThrowException()
        {
            var app = new ChatterClient();
            Assert.Throws<NotConnectedException>(() => app.Disconnect());
        }

        [Fact]
        public async Task SendMessage_WithoutConnection_ShouldThrowException()
        {
            var app = new ChatterClient();
            await Assert.ThrowsAsync<NotConnectedException>(() => app.SendMessageAsync("asdsad"));
        }

        [Fact]
        public async Task GetConnectedUsernames_WithoutConnection_ShouldThrowException()
        {
            var app = new ChatterClient();
            await Assert.ThrowsAsync<NotConnectedException>(() => app.GetConnectedUsernamesAsync());
        }

        [Fact]
        public void Connect_WithoutServer_ShouldThrowUnableToConnectException()
        {
            var app = new ChatterClient();
            Assert.Throws<ConnectException>(() => app.Connect("127.0.0.1", 1234, "asdas"));
        }

        [Fact]
        public void Connect_ServerIsRunning_ShouldConnect()
        {
            var app = new ChatterClient();
            var server = new ChatterServer(new PacketReader(), new PacketWriter(), new NullLogger(new LogFactory()));
            server.Start(IPAddress.Parse("127.0.0.0.1"), 9000);

            app.Connect("127.0.0.1", 9000, "tester");

            Assert.True(app.IsConnected);
            Assert.Single(server.Clients);
            Assert.Contains(server.Clients, c => c.Username == "tester");
        }

        [Fact]
        public void SendMessage_Connected_ShouldReceiveMessage()
        {
            MessageReceivedEventArgs args = null;
            var app = new ChatterClient();
            var server = new ChatterServer(new PacketReader(), new PacketWriter(), new NullLogger(new LogFactory()));
            server.Start(IPAddress.Parse("127.0.0.0.1"), 9000);

            var startTime = DateTime.Now;

            app.Connect("127.0.0.1", 9000, "tester");
            app.OnMessageReceived += (_, a) => args = a;


            string msg = "Mleko";
            app.SendMessageAsync(msg);

            while(args == null)
            {
                Thread.Sleep(1);
            }

            if (args == null)
                throw new Exception();

            Assert.Equal(msg, args.Message);
            Assert.Equal("tester", args.Username);
            Assert.True(args.Time >= startTime);
            Assert.True(args.Time <= DateTime.Now);
        }

        [Fact]
        public void Connect_OtherUserSameUsername_CannotConnect()
        {
            var app = new ChatterClient();
            var other = new ChatterClient();
            var server = new ChatterServer(new PacketReader(), new PacketWriter(), new NullLogger(new LogFactory()));
            server.Start(IPAddress.Parse("127.0.0.0.1"), 9000);


            app.Connect("127.0.0.1", 9000, "tester");
            Assert.Throws<ConnectException>(() => other.Connect("127.0.0.1", 9001, "other"));
        }

        [Fact]
        public void SendMessage_OtherUserWrite_ShouldReceiveMessage()
        {
            MessageReceivedEventArgs args = null;
            var app = new ChatterClient();
            var other = new ChatterClient();
            var server = new ChatterServer(new PacketReader(), new PacketWriter(), new NullLogger(new LogFactory()));
            server.Start(IPAddress.Parse("127.0.0.0.1"), 9000);

            var startTime = DateTime.Now;

            app.Connect("127.0.0.1", 9000, "tester");
            other.Connect("127.0.0.1", 9000, "other");
            app.OnMessageReceived += (_, a) => args = a;


            string msg = "Mleko";
            other.SendMessageAsync(msg);

            while (args == null)
            {
                Thread.Sleep(1);
            }

            if (args == null)
                throw new Exception();

            Assert.Equal(msg, args.Message);
            Assert.Equal("other", args.Username);
            Assert.True(args.Time >= startTime);
            Assert.True(args.Time <= DateTime.Now);
        }

        [Fact]
        public void GetConnectedUsernames_OnlyOne_ReturnsMyNickname()
        {
            var name = "Mleko dwa";
            var app = new ChatterClient();
            var server = new ChatterServer(new PacketReader(), new PacketWriter(), new NullLogger(new LogFactory()));
            server.Start(IPAddress.Parse("127.0.0.0.1"), 9000);

            app.Connect("127.0.0.1", 9000, name);
            var usernames = app.GetConnectedUsernamesAsync().Result;
            Assert.Single(usernames, name);
        }
    }
}
