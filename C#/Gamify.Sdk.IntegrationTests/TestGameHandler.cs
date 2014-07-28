using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.IntegrationTests.Setup;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Sdk.IntegrationTests
{
    public class TestGameHandler
    {
        private static ICollection<TestGameHandler> connectedClients;

        private readonly IGameInitializer gameInitializer;
        private readonly ISerializer serializer;
        private IGameService gameService;

        public event EventHandler<TestGameEventArgs> Notification;

        public string UserName { get; private set; }

        static TestGameHandler()
        {
            connectedClients = new List<TestGameHandler>();
        }

        public TestGameHandler(IGameInitializer gameInitializer, ISerializer serializer)
        {
            this.gameInitializer = gameInitializer;
            this.serializer = serializer;
        }

        public void OnOpen()
        {
            this.Initialize();

            connectedClients.Add(this);
        }

        public void OnMessage(string message)
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

                this.gameService.Connect(playerConnectRequest.PlayerName);
            }
            else
            {
                this.gameService.Send(message);
            }
        }

        public void OnClose()
        {
            this.gameService.Disconnect(this.UserName);
            connectedClients.Remove(this);
        }

        private void Initialize()
        {
            var gameDefinition = new TestGameDefinition(this.serializer);

            this.gameService = gameInitializer.Initialize(gameDefinition);

            this.gameService.Notification += (sender, args) =>
            {
                this.PushMessage(args.Receiver, args.Notification);
            };
        }

        private void PushMessage(string receiver, GameNotification notification)
        {
            var serializedNotification = this.serializer.Serialize(notification);
            var client = connectedClients
                .Cast<TestGameHandler>()
                .FirstOrDefault(c => c.UserName == receiver);

            if (client != null)
            {
                client.Notification(this, new TestGameEventArgs(serializedNotification));
            }
        }
    }
}
