using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Setup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gamify.Sdk.IntegrationTests
{
    [TestClass]
    public class GameIntegrationTests
    {
        private IGameInitializer gameInitializer;
        private ISerializer serializer;

        [TestInitialize]
        public void Initialize()
        {
            this.gameInitializer = new GameInitializer();
            this.serializer = new JsonSerializer();
        }

        [TestMethod]
        public void IT_When_ConnectPlayer_Then_Success()
        {
            var notification = default(GameNotification);
            var notificationObject = default(object);
            var player1Name = GetPlayerName();
            var player1GameHandler = this.ConnectPlayer(player1Name);

            player1GameHandler.Notification += (sender, e) =>
            {
                notification = this.serializer.Deserialize<GameNotification>(e.SerializedNotification);
                notificationObject = this.serializer.Deserialize<object>(notification.SerializedNotificationObject);
            };

            var player2Name = GetPlayerName();
            var player2GameHandler = this.ConnectPlayer(player2Name);

            Assert.AreEqual(player1Name, player1GameHandler.UserName);
            Assert.AreEqual(player2Name, player2GameHandler.UserName);
            Assert.AreEqual((int)GameNotificationType.PlayerConnected, notification.Type);
            Assert.IsNotNull(notificationObject);
            Assert.IsTrue(notificationObject is PlayerConnectedNotificationObject);

            var playerConnectedNotificationObject = notificationObject as PlayerConnectedNotificationObject;

            Assert.AreEqual(player2Name, playerConnectedNotificationObject.PlayerName);
        }

        [TestMethod]
        public void IT_When_CreateGame_Then_Success()
        {
            var player1Name = GetPlayerName();
            var player1GameHandler = this.ConnectPlayer(player1Name);
            var player2Name = GetPlayerName();
            var player2GameHandler = this.ConnectPlayer(player2Name);

            var notification = default(GameNotification);
            var notificationObject = default(object);

            player2GameHandler.Notification += (sender, e) =>
            {
                notification = this.serializer.Deserialize<GameNotification>(e.SerializedNotification);
                notificationObject = this.serializer.Deserialize<object>(notification.SerializedNotificationObject);
            };

            var createGameRequestObject = new CreateGameClientMessage
            {
                UserName = player1Name,
                InvitedUserName = player2Name
            };
            var createGameRequest = new GameRequest(GameRequestType.CreateGame)
            {
                Sender = player1Name,
                SerializedRequestObject = this.serializer.Serialize(createGameRequestObject)
            };

            player1GameHandler.OnMessage(this.serializer.Serialize(createGameRequest));

            Assert.AreEqual((int)GameNotificationType.GameInvite, notification.Type);
            Assert.IsNotNull(notificationObject);
            Assert.IsTrue(notificationObject is GameInviteReceivedServerMessage);

            var gameInviteNotificationObject = notificationObject as GameInviteReceivedServerMessage;

            Assert.AreEqual(player1Name, gameInviteNotificationObject.Player1Name);
            Assert.AreEqual(string.Format("{0}-vs-{1}", player1Name, player2Name), gameInviteNotificationObject.SessionName);
            Assert.IsTrue(string.IsNullOrEmpty(gameInviteNotificationObject.AdditionalInformation));
        }

        [TestMethod]
        public void IT_When_AcceptGame_Then_Success()
        {
            var player1Name = GetPlayerName();
            var player1GameHandler = this.ConnectPlayer(player1Name);
            var player2Name = GetPlayerName();
            var player2GameHandler = this.ConnectPlayer(player2Name);

            var notification = default(GameNotification);
            var notificationObject = default(object);

            player1GameHandler.Notification += (sender, e) =>
            {
                notification = this.serializer.Deserialize<GameNotification>(e.SerializedNotification);
                notificationObject = this.serializer.Deserialize<object>(notification.SerializedNotificationObject);
            };
            player2GameHandler.Notification += (sender, e) =>
            {
                var gameInviteNotification = this.serializer.Deserialize<GameNotification>(e.SerializedNotification);

                if (gameInviteNotification.Type != (int)GameNotificationType.GameInvite)
                {
                    return;
                }

                var gameInviteNotificationObject = this.serializer.Deserialize<GameInviteReceivedServerMessage>(gameInviteNotification.SerializedNotificationObject);

                var acceptGameRequestObject = new AcceptGameClientMessage
                {
                    UserName = player2Name,
                    SessionName = gameInviteNotificationObject.SessionName
                };
                var acceptGameRequest = new GameRequest(GameRequestType.GameAccepted)
                {
                    Sender = player2Name,
                    SerializedRequestObject = this.serializer.Serialize(acceptGameRequestObject)
                };

                player2GameHandler.OnMessage(this.serializer.Serialize(acceptGameRequest));
            };

            var createGameRequestObject = new CreateGameClientMessage
            {
                UserName = player1Name,
                InvitedUserName = player2Name
            };
            var createGameRequest = new GameRequest(GameRequestType.CreateGame)
            {
                Sender = player1Name,
                SerializedRequestObject = this.serializer.Serialize(createGameRequestObject)
            };

            player1GameHandler.OnMessage(this.serializer.Serialize(createGameRequest));

            Assert.AreEqual((int)GameNotificationType.GameCreated, notification.Type);
            Assert.IsNotNull(notificationObject);
            Assert.IsTrue(notificationObject is GameCreatedServerMessage);

            var gameCreatedNotificationObject = notificationObject as GameCreatedServerMessage;

            Assert.AreEqual(player1Name, gameCreatedNotificationObject.Player1Name);
            Assert.AreEqual(player2Name, gameCreatedNotificationObject.Player2Name);
            Assert.AreEqual(string.Format("{0}-vs-{1}", player1Name, player2Name), gameCreatedNotificationObject.SessionName);
            Assert.IsTrue(string.IsNullOrEmpty(gameCreatedNotificationObject.AdditionalInformation));
        }

        [TestMethod]
        public void IT_When_RejectGame_Then_Success()
        {
            var player1Name = GetPlayerName();
            var player1GameHandler = this.ConnectPlayer(player1Name);
            var player2Name = GetPlayerName();
            var player2GameHandler = this.ConnectPlayer(player2Name);

            var notification = default(GameNotification);
            var notificationObject = default(object);

            player1GameHandler.Notification += (sender, e) =>
            {
                notification = this.serializer.Deserialize<GameNotification>(e.SerializedNotification);
                notificationObject = this.serializer.Deserialize<object>(notification.SerializedNotificationObject);
            };
            player2GameHandler.Notification += (sender, e) =>
            {
                var gameInviteNotification = this.serializer.Deserialize<GameNotification>(e.SerializedNotification);

                if (gameInviteNotification.Type != (int)GameNotificationType.GameInvite)
                {
                    return;
                }

                var gameInviteNotificationObject = this.serializer.Deserialize<GameInviteReceivedServerMessage>(gameInviteNotification.SerializedNotificationObject);

                var rejectGameRequestObject = new RejectGameClientMessage
                {
                    UserName = player2Name,
                    SessionName = gameInviteNotificationObject.SessionName
                };
                var rejectGameRequest = new GameRequest(GameRequestType.GameRejected)
                {
                    Sender = player2Name,
                    SerializedRequestObject = this.serializer.Serialize(rejectGameRequestObject)
                };

                player2GameHandler.OnMessage(this.serializer.Serialize(rejectGameRequest));
            };

            var createGameRequestObject = new CreateGameClientMessage
            {
                UserName = player1Name,
                InvitedUserName = player2Name
            };
            var createGameRequest = new GameRequest(GameRequestType.CreateGame)
            {
                Sender = player1Name,
                SerializedRequestObject = this.serializer.Serialize(createGameRequestObject)
            };

            player1GameHandler.OnMessage(this.serializer.Serialize(createGameRequest));

            Assert.AreEqual((int)GameNotificationType.GameRejected, notification.Type);
            Assert.IsNotNull(notificationObject);
            Assert.IsTrue(notificationObject is GameRejectedServerMessage);

            var gameRejectedNotificationObject = notificationObject as GameRejectedServerMessage;

            Assert.AreEqual(player1Name, gameRejectedNotificationObject.Player1Name);
            Assert.AreEqual(player2Name, gameRejectedNotificationObject.Player2Name);
            Assert.AreEqual(string.Format("{0}-vs-{1}", player1Name, player2Name), gameRejectedNotificationObject.SessionName);
        }

        private static string GetPlayerName()
        {
            var playerName = string.Format("player{0}", Guid.NewGuid());

            return playerName;
        }

        private TestGameHandler ConnectPlayer(string playerName)
        {
            var playerConnectRequestObject = new PlayerConnectRequestObject
            {
                PlayerName = playerName
            };
            var playerConnectRequest = new GameRequest(GameRequestType.PlayerConnect)
            {
                Sender = playerName,
                SerializedRequestObject = this.serializer.Serialize(playerConnectRequestObject)
            };
            var playerGameHandler = new TestGameHandler(this.gameInitializer, this.serializer);

            playerGameHandler.OnOpen();
            playerGameHandler.OnMessage(this.serializer.Serialize(playerConnectRequest));

            return playerGameHandler;
        }
    }
}
