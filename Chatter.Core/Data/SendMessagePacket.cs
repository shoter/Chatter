using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Data
{
    [Serializable]
    public class SendMessagePacket : Packet
    {
        public SendMessagePacket(string message, string username, DateTime time)
            :base(PacketType.SendMessage)
        {
            this.Message = message;
            this.Username = username;
            this.Time = time;
        }

        public string Message { get; internal set; }
        public string Username { get; internal set; }
        public DateTime Time { get; internal set; }

    }
}
