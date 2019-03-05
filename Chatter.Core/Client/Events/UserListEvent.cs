using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Client.Events
{
    public class UserListEvent : ClientEvent
    {
        public UserListEvent(List<string> users)
        {
            this.Users = users;
        }

        public List<string> Users { get; set; }
    }
}
