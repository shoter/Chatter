using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Data
{
    [Serializable]
    public abstract class AuthorizedPacket : Packet
    {
        public AuthorizedPacket(PacketType type, string username)
             : base(type)
        {
            this.Username = username;
        }

        public string Username { get; internal set; }
    }
}
