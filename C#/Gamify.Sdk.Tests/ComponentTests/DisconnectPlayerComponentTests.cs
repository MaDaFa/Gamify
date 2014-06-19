using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Gamify.Sdk.UnitTests.ComponentTests
{
    [TestClass]
    public class DisconnectPlayerComponentTests
    {
        private readonly string requestPlayer = "player2";

        private ISerializer serializer;
        private IGamePlayer player1;
        private IGamePlayer player2;
        private Mock<IPlayerService> playerServiceMock;
        private Mock<INotificationService> notificationServiceMock;
        private IGameComponent disconnectPlayerComponent;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();

            this.player1 = new GamePlayer
            {
                DisplayName = "Player 1",
                Name = "player1"
            };
            this.player2 = new GamePlayer
            {
                DisplayName = "Player 2",
                Name = "player2"
            };

            this.playerServiceMock = new Mock<IPlayerService>();

            this.playerServiceMock
                .Setup(s => s.GetAllConnected(It.Is<string>(x => x == this.requestPlayer)))
                .Returns(new List<IGamePlayer> { this.player1 })
                .Verifiable();

            this.notificationServiceMock = new Mock<INotificationService>();

            this.disconnectPlayerComponent = new DisconnectPlayerComponent(playerServiceMock.Object, notificationServiceMock.Object, this.serializer);
        }

        [TestMethod]
        public void When_HandleDisconnectPlayer_Then_Success()
        {
            var disconnectPlayerRequest = new PlayerDisconnectRequestObject
            {
                PlayerName = this.requestPlayer,
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.PlayerDisconnect,
                SerializedRequestObject = this.serializer.Serialize(disconnectPlayerRequest)
            };

            var canHandle = this.disconnectPlayerComponent.CanHandleRequest(gameRequest);

            this.disconnectPlayerComponent.HandleRequest(gameRequest);

            this.playerServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<GameNotificationType>(t => t == GameNotificationType.PlayerDisconnected),
                    It.Is<object>(o => ((PlayerDisconnectedNotificationObject)o).PlayerName == this.requestPlayer),
                    It.Is<string>(x => x == this.player1.Name)));

            Assert.IsTrue(canHandle);
        }
    }
}
