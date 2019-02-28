using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Client.Events
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs()
        {
        }

        public string Username { get; set; }

        public DateTime Time { get; set; }

        public string Message { get; set; }
    }
}
