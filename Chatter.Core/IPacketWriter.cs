using Chatter.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core
{
    public interface IPacketWriter
    {
        byte[] CreatePacket(Packet packet); 
    }
}
