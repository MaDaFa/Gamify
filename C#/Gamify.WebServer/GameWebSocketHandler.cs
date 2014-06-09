using Gamify.Sdk;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;
using Microsoft.Web.WebSockets;
using System;
using System.Linq;

namespace Gamify.WebServer
{
    public class GameWebSocketHandler : WebSocketHandler
    {
        private static WebSocketCollection connectedClients;

        private readonly IGameBuilder gameBuilder;
        private readonly ISerializer serializer;

        private IGameService gameService;

        public string UserName { get; private set; }

        static GameWebSocketHandler()
        {
            connectedClients = new WebSocketCollection();
        }

        public GameWebSocketHandler(IGameBuilder gameBuilder, ISerializer serializer)
        {
            this.gameBuilder = gameBuilder;
            this.serializer = serializer;
        }

        public override void OnOpen()
        {
            this.Initialize();

            connectedClients.Add(this);
        }

        public override void OnMessage(string message)
        {
            var gameRequest = this.serializer.Deserialize<GameRequest>(message);

            if (gameRequest.Type != (int)GameRequestType.PlayerConnect && string.IsNullOrEmpty(this.UserName))
            {
                throw new ApplicationException("A connection error occurred. The players must be connected in order to perform other type of requests");
            }

            if (gameRequest.Type == (int)GameRequestType.PlayerConnect && !string.IsNullOrEmpty(this.UserName))
            {
                throw new ApplicationException("A connection error occurred when trying to connect a player already connected");
            }

            if (gameRequest.Type == (int)GameRequestType.PlayerConnect)
            {
                var playerConnectRequest = this.serializer.Deserialize<PlayerConnectRequestObject>(gameRequest.SerializedRequestObject);

                this.UserName = playerConnectRequest.PlayerName;

                this.gameService.Connect(playerConnectRequest.PlayerName, playerConnectRequest.AccessToken);
            }
            else
            {
                this.gameService.Send(message);
            }
        }

        public override void OnClose()
        {
            base.OnClose();

            connectedClients.Remove(this);
            gameService.Disconnect(this.UserName);
        }

        private void Initialize()
        {
            this.gameService = gameBuilder.Build();

            this.gameService.Notification += (sender, args) =>
            {
                this.PushMessage(args.UserName, args.Notification);
            };
        }

        private void PushMessage(string userName, GameNotification notification)
        {
            var serializedNotification = this.serializer.Serialize(notification);
            var client = connectedClients
                .Cast<GameWebSocketHandler>()
                .FirstOrDefault(c => c.UserName == userName);

            client.Send(serializedNotification);
        }
    }
}