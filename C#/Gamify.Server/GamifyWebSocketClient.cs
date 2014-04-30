using Gamify.Sdk;
using Gamify.Service;
using System.Web.WebSockets;

namespace Gamify.Server
{
    public class GamifyWebSocketClient : IGamifyClient<AspNetWebSocketContext>
    {
        public string Id { get; private set; }

        public IGamePlayer Player { get; set; }

        public AspNetWebSocketContext Context { get; set; }

        public GamifyWebSocketClient(string id)
        {
            this.Id = id;
        }
    }
}
