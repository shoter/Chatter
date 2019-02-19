using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Data
{
    [Serializable]
    public class PeopleListPacket : Packet
    {
        public PeopleListPacket(List<string> usernames)
            : base(PacketType.PeopleList)
        {
            this.Usernames = usernames;
        }

        public List<string> Usernames { get; private set; }

    }
}
