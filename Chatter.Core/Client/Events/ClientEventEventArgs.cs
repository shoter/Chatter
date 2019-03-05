using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Client.Events
{
    public class ClientEventEventArgs
    {
        public ClientEventEventArgs(ClientEvent e)
        {
            this.ClientEvent = e;
        }

        public ClientEvent ClientEvent { get; set; }
    }
}
