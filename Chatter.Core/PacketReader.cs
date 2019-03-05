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
        public Packet ReadPacket(Stream stream) => ReadPacketAsync(stream).Result;


        public async Task<Packet> ReadPacketAsync(Stream stream)
        {
            int size = 0;

            using (var br = new BinaryReader(stream, Encoding.BigEndianUnicode, leaveOpen: true))
            {
                size = br.ReadInt32();
            }

            byte[] data = new byte[size];
            await stream.ReadAsync(data, 0, size);


            using (var ms = new MemoryStream(data))
            {
                BinaryFormatter bf = new BinaryFormatter();
                object obj = bf.Deserialize(ms);
                return (Packet)obj;
            }
        }
    }
}
