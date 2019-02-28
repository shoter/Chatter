using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Data
{
    [Serializable]
    public class SendMessagePacket : AuthorizedPacket
    {
        public SendMessagePacket(string message, string username, DateTime time, string secret)
            :base(PacketType.SendMessage, username, secret)
        {
            this.Message = message;
            this.Time = time;
        }

        public string Message { get; internal set; }
        public DateTime Time { get; internal set; }

    }
}
