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
    public class AcceptGameComponentTests
    {
        private readonly string sessionName = "player1-vs-player2";
        private readonly string requestPlayer = "player2";

        private ISerializer serializer;
        private IGameSession session;
        private Mock<ISessionService> sessionServiceMock;
        private Mock<INotificationService> notificationServiceMock;
        private Mock<ISessionPlayerSetup> playerSetupMock;
        private IGameComponent acceptGameComponent;

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

            this.playerSetupMock = new Mock<ISessionPlayerSetup>();
            this.playerSetupMock.Setup(s => s.GetPlayerReady(It.Is<AcceptGameClientMessage>(x => x.SessionName == this.sessionName),
                It.Is<TestSessionPlayer>(p => p.Information.Name == "player2")))
                .Verifiable();

            this.acceptGameComponent = new GameCreationComponent(sessionServiceMock.Object, notificationServiceMock.Object, this.playerSetupMock.Object, this.serializer);
        }

        [TestMethod]
        public void UT_When_HandleAcceptGame_Then_Success()
        {
            var acceptGameRequest = new AcceptGameClientMessage
            {
                SessionName = this.sessionName,
                UserName = this.requestPlayer,
                AdditionalInformation = "Test"
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.GameAccepted,
                SerializedRequestObject = this.serializer.Serialize(acceptGameRequest)
            };

            var canHandle = this.acceptGameComponent.CanHandleRequest(gameRequest);

            this.acceptGameComponent.HandleRequest(gameRequest);

            this.sessionServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<GameNotificationType>(t => t == GameNotificationType.GameCreated),
                It.Is<object>(o => ((GameCreatedServerMessage)o).SessionName == this.session.Name),
                It.Is<string>(x => x == this.session.Player1Name),
                It.Is<string>(x => x == this.session.Player2Name)));
            this.playerSetupMock.VerifyAll();

            Assert.IsTrue(canHandle);
            Assert.IsTrue(this.session.Player1.PendingToMove);
            Assert.IsFalse(this.session.Player2.PendingToMove);
        }
    }
}
