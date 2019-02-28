using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Data
{
    public class ErrorPacket : Packet
    {
        public ErrorPacket() : base(PacketType.Error)
        {
        }

        public string Error { get; set; }
    }
}
