using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gamify.Sdk.UnitTests.ComponentTests
{
    [TestClass]
    public class OpenGameComponentTests
    {
        private readonly string requestPlayer = "player2";
        private readonly string sessionName = "player1-vs-player2";

        private ISerializer serializer;
        private Mock<ISessionService> sessionServiceMock;
        private Mock<INotificationService> notificationServiceMock;
        private Mock<IGameInformationNotificationFactory<TestMoveObject, TestResponseObject>> gameInformationNotificationFactoryMock;
        private IGameComponent openGameComponent;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();

            var player1 = new TestSessionPlayer()
            {
                SessionName = this.sessionName,
                PendingToMove = false,
                Information = new GamePlayer
                {
                    DisplayName = "Player 1",
                    Name = "player1"
                }
            };
            var player2 = new TestSessionPlayer()
            {
                SessionName = this.sessionName,
                PendingToMove = true,
                Information = new GamePlayer
                {
                    DisplayName = "Player 2",
                    Name = "player2"
                }
            };

            var session = new GameSession(player1, player2);
            var gameInformationNotification = Mock.Of<GameInformationNotificationObject>(n => n.SessionName == this.sessionName);

            this.sessionServiceMock = new Mock<ISessionService>();
            this.sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == this.sessionName)))
                .Returns(session)
                .Verifiable();

            var sessionHistoryServiceMock = new Mock<ISessionHistoryService<TestMoveObject, TestResponseObject>>();

            this.notificationServiceMock = new Mock<INotificationService>();

            var playerHistoryItemFactory = Mock.Of<IPlayerHistoryItemFactory<TestMoveObject, TestResponseObject>>();

            this.gameInformationNotificationFactoryMock = new Mock<IGameInformationNotificationFactory<TestMoveObject, TestResponseObject>>();
            this.gameInformationNotificationFactoryMock
                .Setup(f => f.Create(It.Is<IGameSession>(s => s == session),
                    It.Is<ISessionHistoryService<TestMoveObject, TestResponseObject>>(x => x == sessionHistoryServiceMock.Object),
                    It.Is<IPlayerHistoryItemFactory<TestMoveObject, TestResponseObject>>(x => x == playerHistoryItemFactory)))
                .Returns(gameInformationNotification)
                .Verifiable();

            this.openGameComponent = new OpenGameComponent<TestMoveObject, TestResponseObject>(this.sessionServiceMock.Object, 
                sessionHistoryServiceMock.Object, this.notificationServiceMock.Object,
                this.gameInformationNotificationFactoryMock.Object, playerHistoryItemFactory, this.serializer);
        }

        [TestMethod]
        public void When_HandleOpenGame_Then_Success()
        {
            var openGameRequest = new OpenGameRequestObject
            {
                SessionName = this.sessionName,
                PlayerName = this.requestPlayer
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.OpenGame,
                SerializedRequestObject = this.serializer.Serialize(openGameRequest)
            };

            var canHandle = this.openGameComponent.CanHandleRequest(gameRequest);

            this.openGameComponent.HandleRequest(gameRequest);

            this.sessionServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<GameNotificationType>(t => t == GameNotificationType.SendGameInformation),
                It.Is<object>(o => ((GameInformationNotificationObject)o).SessionName == this.sessionName),
                It.Is<string>(x => x == this.requestPlayer)));
            this.gameInformationNotificationFactoryMock.VerifyAll();

            Assert.IsTrue(canHandle);
        }
    }
}
