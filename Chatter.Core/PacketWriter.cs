using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Chatter.Core.Data;

namespace Chatter.Core
{
    public class PacketWriter : IPacketWriter
    {
        public byte[] CreatePacket(Packet packet)
        {
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, packet);


                using (MemoryStream s = new MemoryStream((int)stream.Length + sizeof(int)))
                using (var bw = new BinaryWriter(s, Encoding.BigEndianUnicode))
                {
                    int size = (int)stream.Length;
                    bw.Write(size);

                    stream.Position = 0;
                    stream.CopyTo(s);


                    return s.ToArray();
                }


            }


        }
    }
}
