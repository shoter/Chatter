using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Client.Exceptions
{
    public class ConnectException : Exception
    {
        public ConnectException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
