using Chatter.Core.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core.Client
{
    public class ChatterClient
    {
        public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

        public bool IsConnected { get; set; }

        public ChatterClient()
        {

        }

        public void Connect(string serverIP, uint port, string username)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {

        }

        public Task SendMessageAsync(string message)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetConnectedUsernamesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
