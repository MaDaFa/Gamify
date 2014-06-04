using Gamify.Sdk.Setup;
using Microsoft.Web.WebSockets;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Gamify.WebServer.Controllers
{
    public abstract class GameApiController : ApiController
    {
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

        protected abstract IGameDefinition GetGameDefinition();

        private WebSocketHandler GetWebSocketHandler()
        {
            var gameDefinition = this.GetGameDefinition();
            var gameWebSocketHandler = new GameWebSocketHandler(gameDefinition);

            return gameWebSocketHandler;
        }
    }
}