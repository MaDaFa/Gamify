using Gamify.Service;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;

namespace Gamify.Server
{
    public abstract class GamifyWebSocketAsyncHandler : IHttpHandler
    {
        private static readonly int dataFrameBufferSize = 10240;
        private static readonly object lockObject = new object();

        protected static IGamifyService gamifyService;

        public bool IsReusable { get { return false; } }

        protected GamifyWebSocketAsyncHandler()
        {
            this.Initialize();
        }

        private void Initialize()
        {
            if (gamifyService == null)
            {
                lock (lockObject)
                {
                    if (gamifyService == null)
                    {
                        gamifyService = this.IntializeGamifyService();

                        gamifyService.SendMessage += (sender, args) =>
                        {
                            this.SendMessage(args.Client, args.Message);
                        };
                    }
                }
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(this.HandleWebSocketRequest);
            }
            else
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }

        protected abstract IGamifyService IntializeGamifyService();

        protected virtual void OnConnecting()
        {
        }

        protected virtual void OnClosing(bool isClientRequest)
        {
        }

        protected virtual void OnClosed()
        {
        }

        private async Task HandleWebSocketRequest(AspNetWebSocketContext context)
        {
            while (context.WebSocket != null && context.WebSocket.State != WebSocketState.Closed)
            {
                if (context.WebSocket.State == WebSocketState.Open)
                {
                    var connectedClientId = this.ConnectClient(context);
                    var dataFrameBuffer = new ArraySegment<byte>(new byte[dataFrameBufferSize]);
                    var receivedResult = await context.WebSocket.ReceiveAsync(dataFrameBuffer, CancellationToken.None);

                    try
                    {
                        switch (receivedResult.MessageType)
                        {
                            case WebSocketMessageType.Text:
                                var receivedMessage = Encoding.UTF8.GetString(dataFrameBuffer.Array, 0, receivedResult.Count);

                                gamifyService.OnReceive(connectedClientId, receivedMessage);
                                break;
                            case WebSocketMessageType.Binary:
                                throw new NotSupportedException("Binary message types are not supported");
                            case WebSocketMessageType.Close:
                                gamifyService.OnDisconnect(connectedClientId);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        gamifyService.OnError(connectedClientId, ex);
                    }
                }
                else
                {
                    switch (context.WebSocket.State)
                    {
                        case WebSocketState.Connecting:
                            this.OnConnecting();
                            break;
                        case WebSocketState.CloseSent:
                            this.OnClosing(false);
                            break;
                        case WebSocketState.CloseReceived:
                            this.OnClosing(true);
                            break;
                        case WebSocketState.Closed:
                            this.OnClosed();
                            break;
                    }
                }
            }
        }

        private string ConnectClient(AspNetWebSocketContext context)
        {
            var connectedClientId = Guid.NewGuid().ToString();
            var connectedClient = new GamifyWebSocketClient(connectedClientId)
            {
                Context = context
            };

            gamifyService.AddClient(connectedClientId, connectedClient);

            return connectedClientId;
        }

        private void SendMessage(IGamifyClientBase client, string message)
        {
            var gamifyClient = client as GamifyWebSocketClient;
            var buffer = Encoding.UTF8.GetBytes(message);

            if (gamifyClient.Context.WebSocket.State == WebSocketState.Open)
            {
                gamifyClient.Context.WebSocket.SendAsync(
                    new ArraySegment<byte>(buffer),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
            }
        }
    }
}
