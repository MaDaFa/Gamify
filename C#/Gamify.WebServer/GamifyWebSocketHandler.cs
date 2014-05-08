using Gamify.Service;
using Microsoft.Web.WebSockets;
using System;
using System.Linq;
using System.Text;

namespace Gamify.WebServer
{
    public abstract class GamifyWebSocketHandler : WebSocketHandler
    {
        private static readonly object lockObject = new object();

        protected static WebSocketCollection connectedClients;
        protected static IGamifyService gamifyService;

        public string UserName { get; private set; }

        static GamifyWebSocketHandler()
        {
            connectedClients = new WebSocketCollection();
        }

        public GamifyWebSocketHandler(string userName)
        {
            this.Initialize(userName);
        }

        private void Initialize(string userName)
        {
            this.UserName = userName;

            if (gamifyService == null)
            {
                lock (lockObject)
                {
                    if (gamifyService == null)
                    {
                        gamifyService = this.IntializeGamifyService();

                        gamifyService.Notification += (sender, args) =>
                        {
                            this.SendMessage(args.UserName, args.Notification);
                        };
                    }
                }
            }
        }

        protected abstract IGamifyService IntializeGamifyService();

        public override void OnOpen()
        {
            connectedClients.Add(this);

            gamifyService.ConnectUser(this.UserName);
        }

        public override void OnMessage(string message)
        {
            try
            {
                gamifyService.OnReceive(this.UserName, message);
            }
            catch (Exception ex)
            {
                gamifyService.OnError(this.UserName, ex);
            }
        }

        public override void OnClose()
        {
            base.OnClose();

            connectedClients.Remove(this);
            gamifyService.OnDisconnect(this.UserName);
        }

        private void SendMessage(string userName, string message)
        {
            var client = connectedClients
                .Cast<GamifyWebSocketHandler>()
                .FirstOrDefault(c => c.UserName == userName);
            var buffer = Encoding.UTF8.GetBytes(message);

            client.Send(message);
        }
    }
}