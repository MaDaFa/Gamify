using Gamify.Sdk;
using System.Web.WebSockets;

namespace Gamify.Service
{
    public class GamifyWebSocketClient : IGamifyClient<AspNetWebSocketContext>
    {
        public IGamePlayer Player { get; set; }

        public AspNetWebSocketContext Context { get; set; }
    }
}
