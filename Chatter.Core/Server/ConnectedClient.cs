using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Server
{
    public class ConnectedClient
    {
        public ConnectedClient(Socket socket, string username)
        {
            this.Socket = socket;
            this.Username = username;
        }

        public string Username { get; }
        public Socket Socket { get; }
    }
}
