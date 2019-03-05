using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Data
{
    public enum PacketType : int
    {
        SendMessage,
        AskForPeople,
        PeopleList,
        Connect,
        Disconnect,
        ConnectSuccessfull,
        ConnectFailed,
        Error,
        DataReceived
    }
}
