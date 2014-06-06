using Gamify.Contracts.Requests;
using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gamify.Sdk.Tests.ComponentTests
{
    [TestClass]
    public class RejectGameComponentTests
    {
        private readonly string sessionName = "player1-vs-player2";
        private readonly string requestPlayer = "player2";

        private ISerializer serializer;
        private IGameSession session;
        private Mock<ISessionService> sessionServiceMock;
        private Mock<INotificationService> notificationServiceMock;
        private IGameComponent rejectGameComponent;

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
                PendingToMove = false,
                Information = new GamePlayer
                {
                    Name = "Player 2",
                    UserName = "player2"
                }
            };

            this.session = new GameSession(player1, player2);

            this.sessionServiceMock = new Mock<ISessionService>();
            this.sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == this.session.Name)))
                .Returns(this.session)
                .Verifiable();

            this.notificationServiceMock = new Mock<INotificationService>();

            this.rejectGameComponent = new RejectGameComponent(sessionServiceMock.Object, notificationServiceMock.Object, this.serializer);
        }

        [TestMethod]
        public void When_HandleAcceptGame_Then_Success()
        {
            var rejectGameRequest = new GameRejectedRequestObject
            {
                SessionName = this.sessionName,
                PlayerName = this.requestPlayer
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.GameRejected,
                SerializedRequestObject = this.serializer.Serialize(rejectGameRequest)
            };

            var canHandle = this.rejectGameComponent.CanHandleRequest(gameRequest);

            this.rejectGameComponent.HandleRequest(gameRequest);

            this.sessionServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<GameNotificationType>(t => t == GameNotificationType.GameRejected),
                It.Is<object>(o => ((GameRejectedNotificationObject)o).SessionName == this.session.Name
                    && ((GameRejectedNotificationObject)o).Player1Name == this.session.Player1.Information.UserName
                    && ((GameRejectedNotificationObject)o).Player2Name == this.session.Player2.Information.UserName),
                It.Is<string>(x => x == this.session.Player1.Information.UserName)));

            Assert.IsTrue(canHandle);
        }
    }
}
