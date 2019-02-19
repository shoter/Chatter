using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Server.Exceptions
{
    public class ChatterServerException : Exception
    {
        public ChatterServerException()
        {
        }

        public ChatterServerException(string message) : base(message)
        {
        }

        public ChatterServerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ChatterServerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
