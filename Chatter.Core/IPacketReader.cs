using Chatter.Core.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chatter.Core
{
    public interface IPacketReader
    {
        Packet ReadPacket(Stream stream);
    }
}
