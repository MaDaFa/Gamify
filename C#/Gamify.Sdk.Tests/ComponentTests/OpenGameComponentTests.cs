using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Contracts.ClientMessages;
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
        private Mock<ISessionHistoryService<TestMoveObject, TestResponseObject>>  sessionHistoryServiceMock;
        private Mock<INotificationService> notificationServiceMock;
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
            var gameInformationNotification = Mock.Of<GameInformationReceivedServerMessage>(n => n.SessionName == this.sessionName);

            this.sessionServiceMock = new Mock<ISessionService>();
            this.sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == this.sessionName)))
                .Returns(session)
                .Verifiable();

            var player1SessionHistory = new SessionHistory<TestMoveObject, TestResponseObject>(this.sessionName, player1.Information.Name);
            var player2SessionHistory = new SessionHistory<TestMoveObject, TestResponseObject>(this.sessionName, player2.Information.Name);
            
            this.sessionHistoryServiceMock = new Mock<ISessionHistoryService<TestMoveObject, TestResponseObject>>();
            this.sessionHistoryServiceMock
                .Setup(s => s.GetBySessionPlayer(It.Is<string>(x => x == this.sessionName), It.Is<string>(x => x == player1.Information.Name)))
                .Returns(player1SessionHistory)
                .Verifiable();
            this.sessionHistoryServiceMock
                .Setup(s => s.GetBySessionPlayer(It.Is<string>(x => x == this.sessionName), It.Is<string>(x => x == player2.Information.Name)))
                .Returns(player2SessionHistory)
                .Verifiable();

            this.notificationServiceMock = new Mock<INotificationService>();

            var playerHistoryItemFactory = Mock.Of<IPlayerHistoryItemFactory<TestMoveObject, TestResponseObject>>();

            this.openGameComponent = new GameSelectionComponent<TestMoveObject, TestResponseObject>(this.sessionServiceMock.Object, 
                sessionHistoryServiceMock.Object, this.notificationServiceMock.Object, playerHistoryItemFactory, this.serializer);
        }

        [TestMethod]
        public void UT_When_HandleOpenGame_Then_Success()
        {
            var openGameRequest = new OpenGameClientMessage
            {
                SessionName = this.sessionName,
                UserName = this.requestPlayer
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
                It.Is<object>(o => ((GameInformationReceivedServerMessage)o).SessionName == this.sessionName),
                It.Is<string>(x => x == this.requestPlayer)));
            this.sessionHistoryServiceMock.VerifyAll();

            Assert.IsTrue(canHandle);
        }
    }
}
