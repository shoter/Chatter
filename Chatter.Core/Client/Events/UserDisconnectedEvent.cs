using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Client.Events
{
    public class UserDisconnectedEvent : ClientEvent
    {
        public UserDisconnectedEvent(string username)
        {
            this.Username = username;
        }

        public string Username { get; set; }
    }
}
