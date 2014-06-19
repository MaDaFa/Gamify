using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gamify.Sdk.UnitTests.ComponentTests
{
    [TestClass]
    public class AbandonGameComponentTests
    {
        private readonly string sessionName = "player1-vs-player2";
        private readonly string requestPlayer = "player2";

        private ISerializer serializer;
        private IGameSession session;
        private Mock<ISessionService> sessionServiceMock;
        private Mock<INotificationService> notificationServiceMock;
        private IGameComponent abandonGameComponent;

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

            this.session = new GameSession(player1, player2);

            this.sessionServiceMock = new Mock<ISessionService>();
            this.sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == this.session.Name)))
                .Returns(this.session)
                .Verifiable();
            this.sessionServiceMock
                .Setup(s => s.Abandon(It.Is<string>(x => x == this.session.Name)))
                .Verifiable();

            this.notificationServiceMock = new Mock<INotificationService>();

            this.abandonGameComponent = new AbandonGameComponent(sessionServiceMock.Object, notificationServiceMock.Object, this.serializer);
        }

        [TestMethod]
        public void When_HandleAbandonGame_Then_Success()
        {
            var abandonGameRequest = new AbandonGameRequestObject
            {
                SessionName = this.sessionName,
                PlayerName = this.requestPlayer
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.AbandonGame,
                SerializedRequestObject = this.serializer.Serialize(abandonGameRequest)
            };

            var canHandle = this.abandonGameComponent.CanHandleRequest(gameRequest);

            this.abandonGameComponent.HandleRequest(gameRequest);

            this.sessionServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<GameNotificationType>(t => t == GameNotificationType.GameAbandoned),
                    It.Is<object>(o => ((GameAbandonedNotificationObject)o).SessionName == this.session.Name),
                    It.Is<string>(x => x == this.session.Player1Name),
                    It.Is<string>(x => x == this.session.Player2Name)));

            Assert.IsTrue(canHandle);
        }
    }
}
