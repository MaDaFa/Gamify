using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Setup;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

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
            var player1Name = "player1";
            var player1GameHandler = this.ConnectPlayer(player1Name);

            player1GameHandler.Notification += (sender, e) =>
            {
                notification = this.serializer.Deserialize<GameNotification>(e.SerializedNotification);
                notificationObject = this.serializer.Deserialize<object>(notification.SerializedNotificationObject);
            };

            var player2Name = "player2";
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
            var player1Name = "player1";
            var player1GameHandler = this.ConnectPlayer(player1Name);
            var player2Name = "player2";
            var player2GameHandler = this.ConnectPlayer(player2Name);

            var notification = default(GameNotification);
            var notificationObject = default(object);

            player2GameHandler.Notification += (sender, e) =>
            {
                notification = this.serializer.Deserialize<GameNotification>(e.SerializedNotification);
                notificationObject = this.serializer.Deserialize<object>(notification.SerializedNotificationObject);
            };

            var createGameRequestObject = new CreateGameRequestObject
            {
                PlayerName = player1Name,
                InvitedPlayerName = player2Name
            };
            var createGameRequest = new GameRequest(GameRequestType.CreateGame)
            {
                Sender = player1Name,
                SerializedRequestObject = this.serializer.Serialize(createGameRequestObject)
            };

            player1GameHandler.OnMessage(this.serializer.Serialize(createGameRequest));

            Assert.AreEqual((int)GameNotificationType.GameInvite, notification.Type);
            Assert.IsNotNull(notificationObject);
            Assert.IsTrue(notificationObject is GameInviteNotificationObject);

            var gameInviteNotificationObject = notificationObject as GameInviteNotificationObject;

            Assert.AreEqual(player1Name, gameInviteNotificationObject.Player1Name);
            Assert.AreEqual(string.Format("{0}-vs-{1}", player1Name, player2Name), gameInviteNotificationObject.SessionName);
            Assert.IsTrue(string.IsNullOrEmpty(gameInviteNotificationObject.AdditionalInformation));
        }

        private TestGameHandler ConnectPlayer(string playerName)
        {
            var playerConnectRequestObject = new PlayerConnectRequestObject
            {
                PlayerName = playerName,
                AccessToken = Guid.NewGuid().ToString()
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
