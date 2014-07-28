using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Services;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Gamify.Sdk.UnitTests.ServiceTests
{
    [TestClass]
    public class GameServiceTests
    {
        private ISerializer serializer;
        private IGameService gameService;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();
            this.gameService = new GameService(this.serializer);
        }

        [TestMethod]
        public void UT_When_Connect_Then_Success()
        {
            var userName = "player1";
            var notification = new GameNotification
            {
                Type = (int)GameNotificationType.PlayerConnected,
                SerializedNotificationObject = "Test"
            };
            var connectComponentMock = new Mock<IGameComponent>();

            connectComponentMock
                .Setup(c => c.CanHandleRequest(It.Is<GameRequest>(r => r.Type == (int)GameRequestType.PlayerConnect)))
                .Returns(true)
                .Verifiable();
            connectComponentMock
                .Setup(c => c.HandleRequest(It.Is<GameRequest>(r => r.Type == (int)GameRequestType.PlayerConnect)))
                .Verifiable();

            this.gameService.RegisterComponent(connectComponentMock.Object);
            this.gameService.Connect(userName);

            connectComponentMock.VerifyAll();
        }

        [TestMethod]
        public void UT_When_SendMessage_Then_Success()
        {
            var testRequestType = 144;
            var userName = "player1";

            var testRequestObject = new TestRequestObject
            {
                PlayerName = userName,
                TestValue = "Test Value 1"
            };
            var request = new GameRequest
            {
                Type = testRequestType,
                SerializedRequestObject = this.serializer.Serialize(testRequestObject)
            };

            var testComponentMock = new Mock<IGameComponent>();

            testComponentMock
                .Setup(c => c.CanHandleRequest(It.Is<GameRequest>(r => r.Type == testRequestType)))
                .Returns(true)
                .Verifiable();
            testComponentMock
                .Setup(c => c.HandleRequest(It.Is<GameRequest>(r => r.Type == testRequestType)))
                .Verifiable();

            this.gameService.RegisterComponent(testComponentMock.Object);

            var serializedMessage = this.serializer.Serialize(request);

            this.gameService.Send(serializedMessage);

            testComponentMock.VerifyAll();
        }

        [TestMethod]
        public void UT_When_Disconnect_Then_Success()
        {
            var userName = "player1";
            var notification = new GameNotification
            {
                Type = (int)GameNotificationType.PlayerDisconnected,
                SerializedNotificationObject = "Test"
            };
            var disconnectComponentMock = new Mock<IGameComponent>();

            disconnectComponentMock
                .Setup(c => c.CanHandleRequest(It.Is<GameRequest>(r => r.Type == (int)GameRequestType.PlayerDisconnect)))
                .Returns(true)
                .Verifiable();
            disconnectComponentMock
                .Setup(c => c.HandleRequest(It.Is<GameRequest>(r => r.Type == (int)GameRequestType.PlayerDisconnect)))
                .Verifiable();

            this.gameService.RegisterComponent(disconnectComponentMock.Object);
            this.gameService.Disconnect(userName);

            disconnectComponentMock.VerifyAll();
        }
    }
}
