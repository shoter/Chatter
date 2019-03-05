using Chatter.Core.Server;
using Chatter.Core.Server.Exceptions;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Chatter.Core.Tests.Unit.Server
{
    public class ChatterServerUnitTests
    {
        [Fact]
        public void Start_CannotStartTwice_ShouldThrowException()
        {
            var server = new ChatterServer(new PacketReader(), new PacketWriter(), new NullLogger(new LogFactory()));
            server.Start(IPAddress.Loopback, 9000);
            Assert.Throws<ChatterServerException>(() => server.Start(IPAddress.Loopback, 9000));
        }
    }
}
