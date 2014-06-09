using Gamify.Sdk;
using Gamify.Sdk.Setup;
using Microsoft.Web.WebSockets;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Gamify.WebServer.Controllers
{
    public class PlayController : ApiController
    {
        private readonly IGameBuilder gameBuilder;
        private ISerializer serializer;

        public PlayController(IGameBuilder gameBuilder, ISerializer serializer)
        {
            this.gameBuilder = gameBuilder;
            this.serializer = serializer;
        }

        public HttpResponseMessage Get()
        {
            var responseCode = HttpStatusCode.BadRequest;

            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(this.GetWebSocketHandler());
                responseCode = HttpStatusCode.SwitchingProtocols;
            }

            return new HttpResponseMessage(responseCode);
        }

        private WebSocketHandler GetWebSocketHandler()
        {
            var gameWebSocketHandler = new GameWebSocketHandler(this.gameBuilder, this.serializer);

            return gameWebSocketHandler;
        }
    }
}