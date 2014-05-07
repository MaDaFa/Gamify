using Microsoft.Web.WebSockets;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Gamify.WebServer.Controllers
{
    public abstract class GamifyController : ApiController
    {
        public HttpResponseMessage Get(string userName)
        {
            var responseCode = HttpStatusCode.BadRequest;

            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(this.GetWebSocketHandler(userName));
                responseCode = HttpStatusCode.SwitchingProtocols;
            }

            return new HttpResponseMessage(responseCode);
        }

        protected abstract WebSocketHandler GetWebSocketHandler(string userName);
    }
}