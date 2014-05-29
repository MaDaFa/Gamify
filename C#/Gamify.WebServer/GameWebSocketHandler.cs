using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Service.Interfaces;
using Microsoft.Web.WebSockets;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.WebServer
{
    public abstract class GameWebSocketHandler : WebSocketHandler
    {
        protected static WebSocketCollection connectedClients;

        private bool isConfigured;
        private bool isConnected;
        private ISerializer<GameRequest> requestSerializer;
        private ISerializer<GameNotification> notificationSerializer;
        private IGameService gameService;

        public string UserName { get; private set; }

        static GameWebSocketHandler()
        {
            connectedClients = new WebSocketCollection();
        }

        public GameWebSocketHandler()
        {
            this.requestSerializer = new JsonSerializer<GameRequest>();
            this.notificationSerializer = new JsonSerializer<GameNotification>();
        }

        public void Initialize(IGameService gameService)
        {
            this.gameService = gameService;
            this.gameService.Notification += (sender, args) =>
            {
                this.PushMessage(args.UserName, args.Notification);
            };

            var gameConfigurators = this.GetGameConfigurators();

            foreach (var gameConfigurator in gameConfigurators)
            {
                gameConfigurator.Configure(gameService);
            }

            this.isConfigured = true;
        }

        protected abstract IEnumerable<IGameConfigurator> GetGameConfigurators();

        public override void OnOpen()
        {
            this.ValidateInitialization();

            connectedClients.Add(this);
        }

        public override void OnMessage(string message)
        {
            this.ValidateInitialization();
            this.ValidateConnection(message);

            gameService.Send(message);
        }

        public override void OnClose()
        {
            this.ValidateInitialization();

            base.OnClose();

            connectedClients.Remove(this);
            gameService.Disconnect(this.UserName);
        }

        private void PushMessage(string userName, GameNotification notification)
        {
            var serializedNotification = this.notificationSerializer.Serialize(notification);
            var client = connectedClients
                .Cast<GameWebSocketHandler>()
                .FirstOrDefault(c => c.UserName == userName);

            client.Send(serializedNotification);
        }

        private void ValidateInitialization()
        {
            if (!isConfigured)
            {
                throw new ApplicationException("The web socket handler must be explicitly initialized before using it");
            }
        }

        private void ValidateConnection(string message)
        {
            var gameRequest = this.requestSerializer.Deserialize(message);

            if (gameRequest.Type == (int)GameRequestType.PlayerConnect && this.isConnected)
            {
                throw new ApplicationException("A connection error occurred when trying to connect a player already connected");
            }

            if (gameRequest.Type != (int)GameRequestType.PlayerConnect && !this.isConnected)
            {
                throw new ApplicationException("A connection error occurred. The players must be connected in order to perform other type of requests");
            }
        }
    }
}