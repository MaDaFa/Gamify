using Gamify.Contracts.Notifications;
using Gamify.Core;
using Gamify.Core.Interfaces;
using Gamify.Service;
using Microsoft.Web.WebSockets;
using System.Linq;

namespace Gamify.WebServer
{
    public abstract class GameWebSocketHandler : WebSocketHandler
    {
        private static readonly object lockObject = new object();

        protected static WebSocketCollection connectedClients;
        protected static IGameService gameService;

        private readonly ISerializer<object> serializer;

        public string UserName { get; private set; }

        static GameWebSocketHandler()
        {
            connectedClients = new WebSocketCollection();
        }

        public GameWebSocketHandler(string userName)
        {
            this.serializer = new JsonSerializer<object>();
            this.UserName = userName;

            this.CheckServiceInitialization();
        }

        private void CheckServiceInitialization()
        {
            if (gameService == null)
            {
                lock (lockObject)
                {
                    if (gameService == null)
                    {
                        gameService = this.IntializeGameService();

                        gameService.Notification += (sender, args) =>
                        {
                            this.SendMessage(args.UserName, args.Notification);
                        };
                    }
                }
            }
        }

        protected abstract IGameService IntializeGameService();

        public override void OnOpen()
        {
            connectedClients.Add(this);

            gameService.ConnectUser(this.UserName);
        }

        public override void OnMessage(string message)
        {
            gameService.ReceiveMessage(message);
        }

        public override void OnClose()
        {
            base.OnClose();

            connectedClients.Remove(this);
            gameService.DisconnectUser(this.UserName);
        }

        private void SendMessage(string userName, GameNotification notification)
        {
            var serializedNotification = this.serializer.Serialize(notification);
            var client = connectedClients
                .Cast<GameWebSocketHandler>()
                .FirstOrDefault(c => c.UserName == userName);

            client.Send(serializedNotification);
        }
    }
}