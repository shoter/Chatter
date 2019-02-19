using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Server
{
    public class ConnectedClient
    {
        public string Username { get; set; }
        public string Ip { get; set; }
        public uint Port { get; set; }
    }
}
