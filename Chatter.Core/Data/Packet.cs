using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Chatter.Core.Data
{
    [Serializable]
    public abstract class Packet
    {
        public Packet(PacketType type)
        {
            this.PacketType = type;
        }

        public static Dictionary<PacketType, Type> PacketTypeToType = new Dictionary<PacketType, Type>()
        {
            { PacketType.Connect, typeof(ConnectPacket) },
            { PacketType.Disconnect, typeof(DisconnectPacket) },
            {PacketType.PeopleList, typeof(PeopleListPacket) },
            {PacketType.SendMessage, typeof(SendMessagePacket) },
            {PacketType.AskForPeople, typeof(AskForPeoplePacket) },
            {PacketType.ConnectSuccessfull, typeof(ConnectSuccessfullPacket) },
            {PacketType.ConnectFailed, typeof(ConnectFailed) },
        };

        public PacketType PacketType { get; internal set; }
    }
}
