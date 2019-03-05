using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Data
{
    [Serializable]
    public class ErrorPacket : Packet
    {
        public ErrorPacket() : base(PacketType.Error)
        {
        }
        public ErrorPacket(string error) : this()
        {
            this.Error = error;
        }
        public ErrorPacket(string error, PacketType packetType)
            :base(packetType)
        {
            this.Error = error;
        }
        public string Error { get; internal set; }
    }
}
