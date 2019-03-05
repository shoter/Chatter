using Chatter.Core.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Chatter.Core.Tests.Unit
{
    public class ChatterApplicationUnitTests
    {
        [Fact]
        public void Connect_WrongPort_ShouldThrowException()
        {
            var app = new ChatterClient(new PacketReader(), new PacketWriter());
            Assert.Throws<ArgumentException>(() => app.Connect("127.0.0.1", 65536, "asdas"));
        }

        [Fact]
        public void Connect_EmptyUsername_ShouldThrowException()
        {
            var app = new ChatterClient(new PacketReader(), new PacketWriter());
            Assert.Throws<ArgumentException>(() => app.Connect("127.0.0.1", 123, string.Empty));
        }
        
        [Fact]
        public void Connect_WhitespaceUsername_ShouldThrowException()
        {
            var app = new ChatterClient(new PacketReader(), new PacketWriter());
            Assert.Throws<ArgumentException>(() => app.Connect("127.0.0.1", 123, "   "));
        }
    }
}
