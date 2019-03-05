using Chatter.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Chatter.Core.Tests.Integration
{
    public class PacketReadWriteTests
    {
        private readonly IPacketReader packetReader = new PacketReader();
        private readonly IPacketWriter packetWriter = new PacketWriter();

        [Fact]
        public void SendMessagePacket_ShouldBeWriteableAndReadable()
        {
            var packet = new SendMessagePacket("Test message", "tester", DateTime.Now);
            var read = createAndRead(packet);

            Assert.Equal(packet.Message, read.Message);
            Assert.Equal(packet.Time, read.Time);
            Assert.Equal(packet.Username, read.Username);
        }

        [Fact]
        public void PeopleListPacket_ShouldBeWriteableAndReadable()
        {
            var packet = new PeopleListPacket(new List<string>()
            {
                "Zosia",
                "Angela",
                "ゆみ"
            });

            var read = createAndRead(packet);

            // If names would be read in different order or smth.
            var packetUsers = packet.Usernames.OrderBy(u => u);
            var readUsers = read.Usernames.OrderBy(u => u);

            Assert.Equal(packetUsers, readUsers);
        }

        [Fact]
        public void DisconnectPacket_ShouldBeWriteableAndReadable()
        {
            var packet = new DisconnectPacket("username");
            DisconnectPacket read = createAndRead(packet);

            Assert.Equal(packet.Username, read.Username);
        }

        [Fact]
        public void AskForPeoplePacket_ShoudBeWriteableAndReadable()
        {
            var packet = new AskForPeoplePacket("username");
            AskForPeoplePacket read = createAndRead(packet);

            Assert.Equal(packet.Username, read.Username);
        }

        [Fact]
        public void ConnectFailed_ShouldBeWriteableAndReadable()
        {
            var packet = new ConnectFailed("You did not know the code");
            ConnectFailed read = createAndRead(packet);

            Assert.Equal(packet.Error, read.Error);
        }

        [Fact]
        public void ConnectPacket_ShouldBeWriteableAndReadable()
        {
            var packet = new ConnectPacket("avaJ sucks");
            ConnectPacket read = createAndRead(packet);

            Assert.Equal(packet.Username, read.Username);
        }

        private TPacket createAndRead<TPacket>(TPacket packet)
            where TPacket : Packet
        {
            byte[] bytes = packetWriter.CreatePacket(packet);
            using (var stream = new MemoryStream(bytes))
                return packetReader.ReadPacket(stream) as TPacket;
        }

    }
}
