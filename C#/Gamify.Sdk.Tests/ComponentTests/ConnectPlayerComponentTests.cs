using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Gamify.Sdk.Tests.ComponentTests
{
    [TestClass]
    public class ConnectPlayerComponentTests
    {
        private readonly string requestPlayer = "player2";

        private ISerializer serializer;
        private IGamePlayer player1;
        private IGamePlayer player2;
        private Mock<IPlayerService> playerServiceMock;
        private Mock<INotificationService> notificationServiceMock;
        private IGameComponent connectPlayerComponent;

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
                .Returns(new List<IGamePlayer> { this.player2 })
                .Verifiable();

            this.notificationServiceMock = new Mock<INotificationService>();

            this.connectPlayerComponent = new ConnectPlayerComponent(playerServiceMock.Object, notificationServiceMock.Object, this.serializer);
        }

        [TestMethod]
        public void When_HandleConnectPlayer_Then_Success()
        {
            var connectPlayerRequest = new PlayerConnectRequestObject
            {
                PlayerName = this.requestPlayer,
                AccessToken = Guid.NewGuid().ToString()
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.PlayerConnect,
                SerializedRequestObject = this.serializer.Serialize(connectPlayerRequest)
            };

            this.playerServiceMock
                .Setup(s => s.Connect(It.Is<string>(x => x == this.requestPlayer), It.IsAny<string>()))
                .Verifiable();

            var canHandle = this.connectPlayerComponent.CanHandleRequest(gameRequest);

            this.connectPlayerComponent.HandleRequest(gameRequest);

            this.playerServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<GameNotificationType>(t => t == GameNotificationType.PlayerConnected),
                    It.Is<object>(o => ((PlayerConnectedNotificationObject)o).PlayerName == this.requestPlayer),
                    It.Is<string>(x => x == this.player2.Name)));

            Assert.IsTrue(canHandle);
        }
    }
}
