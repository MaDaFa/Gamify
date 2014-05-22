using Gamify.Contracts.Notifications;
using Gamify.Core.Interfaces;
using Gamify.Service;
using Microsoft.Web.WebSockets;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.WebServer
{
    public abstract class GameWebSocketHandler : WebSocketHandler
    {
        private static readonly object lockObject = new object();

        protected static WebSocketCollection connectedClients;

        protected ISerializer<object> serializer;
        protected IGameService gameService;

        public string UserName { get; private set; }

        static GameWebSocketHandler()
        {
            connectedClients = new WebSocketCollection();
        }

        public GameWebSocketHandler(string userName, IGameService gameService)
        {
            this.gameService = gameService;

            this.UserName = userName;

            this.ConfigureGameService();
        }

        private void ConfigureGameService()
        {
            this.gameService.Notification += (sender, args) =>
            {
                SendMessage(args.UserName, args.Notification);
            };

            var gameConfigurators = this.GetGameConfigurators();

            foreach (var gameConfigurator in gameConfigurators)
            {
                gameConfigurator.Configure(gameService);
            }
        }

        protected abstract IEnumerable<IGameConfigurator> GetGameConfigurators();

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