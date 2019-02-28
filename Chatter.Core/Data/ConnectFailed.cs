using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Data
{
    [Serializable]
    public class ConnectFailed : Packet
    {
        public ConnectFailed(string message)
            :base(PacketType.ConnectFailed)
        {
            this.ErrorMessage = message;
        }
        public string ErrorMessage { get; internal set; }
    }
}
