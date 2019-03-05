﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Data
{
    [Serializable]
    public class AskForPeoplePacket : AuthorizedPacket
    {
        public AskForPeoplePacket(string username)
            :base(PacketType.AskForPeople, username)
        { }
    }
}
