using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Data
{
    [Serializable]
    public class ConnectPacket : Packet
    {
        public ConnectPacket(string username)
            :base(PacketType.Connect)
        {
            this.Username = username;
        }

        public string Username { get; private set; }
    }
}
