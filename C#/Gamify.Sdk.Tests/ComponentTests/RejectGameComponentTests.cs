using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gamify.Sdk.UnitTests.ComponentTests
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
                PendingToMove = false,
                Information = new GamePlayer
                {
                    DisplayName = "Player 2",
                    Name = "player2"
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
        public void UT_When_HandleAcceptGame_Then_Success()
        {
            var rejectGameRequest = new RejectGameClientMessage
            {
                SessionName = this.sessionName,
                UserName = this.requestPlayer
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
                It.Is<object>(o => ((GameRejectedServerMessage)o).SessionName == this.session.Name
                    && ((GameRejectedServerMessage)o).Player1Name == this.session.Player1Name
                    && ((GameRejectedServerMessage)o).Player2Name == this.session.Player2Name),
                It.Is<string>(x => x == this.session.Player1Name)));

            Assert.IsTrue(canHandle);
        }
    }
}
