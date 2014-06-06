using Gamify.Contracts.Requests;
using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;
using Gamify.Sdk.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gamify.Sdk.Tests.ComponentTests
{
    [TestClass]
    public class OpenGameComponentTests
    {
        private readonly string requestPlayer = "player2";
        private readonly string sessionName = "player1-vs-player2";

        private ISerializer serializer;
        private IGameSession session;
        private GameInformationNotificationObject gameInformationNotification;
        private Mock<ISessionService> sessionServiceMock;
        private Mock<INotificationService> notificationServiceMock;
        private Mock<IGameInformationNotificationFactory> gameInformationNotificationFactoryMock;
        private IGameComponent openGameComponent;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();

            var sessionHistoryService = Mock.Of<ISessionHistoryService<TestMoveObject, TestResponseObject>>();

            var player1 = new TestSessionPlayer(sessionHistoryService)
            {
                SessionName = this.sessionName,
                PendingToMove = false,
                Information = new GamePlayer
                {
                    Name = "Player 1",
                    UserName = "player1"
                }
            };
            var player2 = new TestSessionPlayer(sessionHistoryService)
            {
                SessionName = this.sessionName,
                PendingToMove = true,
                Information = new GamePlayer
                {
                    Name = "Player 2",
                    UserName = "player2"
                }
            };

            this.session = new GameSession(player1, player2);

            this.gameInformationNotification = Mock.Of<GameInformationNotificationObject>(n => n.SessionName == this.sessionName);

            this.sessionServiceMock = new Mock<ISessionService>();
            this.sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == this.sessionName)))
                .Returns(this.session)
                .Verifiable();

            this.notificationServiceMock = new Mock<INotificationService>();

            this.gameInformationNotificationFactoryMock = new Mock<IGameInformationNotificationFactory>();
            this.gameInformationNotificationFactoryMock
                .Setup(f => f.Create(It.Is<IGameSession>(s => s == this.session)))
                .Returns(this.gameInformationNotification)
                .Verifiable();

            this.openGameComponent = new OpenGameComponent(sessionServiceMock.Object, notificationServiceMock.Object, 
                this.gameInformationNotificationFactoryMock.Object, this.serializer);
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
