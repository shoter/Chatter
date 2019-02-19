using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Chatter.Core.Data;

namespace Chatter.Core
{
    public class PacketReader : IPacketReader
    {
        public Packet ReadPacket(Stream stream)
        {
            /*
            byte[] buffer = new byte[16 * 64];

            int read;
            int offset = 0;

            do
            {
                read = stream.Read(buffer, offset, 64);
                offset += read;
            } while (read > 0);*/

            int size = 0;

            using (var br = new BinaryReader(stream, Encoding.BigEndianUnicode, leaveOpen: true))
            {
                size = br.ReadInt32();
            }

            byte[] data = new byte[size];
            stream.Read(data, 0, size);


            using (var ms = new MemoryStream(data))
            {
                BinaryFormatter bf = new BinaryFormatter();
                object obj = bf.Deserialize(ms);
                return (Packet)obj;
            }
        }
    }
}
