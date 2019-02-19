using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Client.Exceptions
{
    public class NotConnectedException : Exception
    {
        public NotConnectedException(string message) : base(message)
        {
        }
    }
}
