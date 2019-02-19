using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Data
{
    [Serializable]
    public class ConnectSuccessfullPacket : Packet
    {
        public ConnectSuccessfullPacket()
        :base(PacketType.ConnectSuccessfull)
        {
        }

        public string Secret { get; set; }
    }
}
