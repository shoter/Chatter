using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Client.Events
{
    public class MessageReceivedEvent : ClientEvent
    {
        public MessageReceivedEvent(string username, string msg)
        {
            this.Username = username;
            this.Message = msg;
        }
        public string Message { get; set; }
        public string Username { get; set; }
    }
}
