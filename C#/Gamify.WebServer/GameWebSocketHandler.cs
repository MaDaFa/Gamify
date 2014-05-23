using Gamify.Contracts.Notifications;
using Gamify.Core;
using Gamify.Core.Interfaces;
using Gamify.Service;
using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.WebServer
{
    public abstract class GameWebSocketHandler : WebSocketHandler
    {
        protected static WebSocketCollection connectedClients;

        private bool isInitialized;
        private ISerializer<object> serializer;
        private IGameService gameService;

        public string UserName { get; private set; }

        static GameWebSocketHandler()
        {
            connectedClients = new WebSocketCollection();
        }

        public GameWebSocketHandler(string userName)
        {
            this.serializer = new JsonSerializer<object>();

            this.UserName = userName;
        }

        public void Initialize(IGameService gameService)
        {
            this.gameService = gameService;
            this.gameService.Notification += (sender, args) =>
            {
                SendMessage(args.UserName, args.Notification);
            };

            var gameConfigurators = this.GetGameConfigurators();

            foreach (var gameConfigurator in gameConfigurators)
            {
                gameConfigurator.Configure(gameService);
            }

            this.isInitialized = true;
        }

        protected abstract IEnumerable<IGameConfigurator> GetGameConfigurators();

        public override void OnOpen()
        {
            this.ValidateInitialization();

            connectedClients.Add(this);

            gameService.ConnectUser(this.UserName);
        }

        public override void OnMessage(string message)
        {
            this.ValidateInitialization();

            gameService.ReceiveMessage(message);
        }

        public override void OnClose()
        {
            this.ValidateInitialization();

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

        private void ValidateInitialization()
        {
            if (!isInitialized)
            {
                throw new ApplicationException("The web socket handler must be explicitly initialized before using it");
            }
        }
    }
}