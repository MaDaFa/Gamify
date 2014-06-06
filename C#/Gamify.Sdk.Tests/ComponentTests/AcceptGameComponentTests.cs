using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;
using Gamify.Sdk.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gamify.Sdk.Tests.ComponentTests
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

            this.playerSetupMock = new Mock<ISessionPlayerSetup>();
            this.playerSetupMock.Setup(s => s.GetPlayerReady(It.Is<GameAcceptedRequestObject>(x => x.SessionName == this.sessionName),
                It.Is<TestSessionPlayer>(p => p.Information.UserName == "player2")))
                .Verifiable();

            this.acceptGameComponent = new AcceptGameComponent(sessionServiceMock.Object, notificationServiceMock.Object, this.playerSetupMock.Object, this.serializer);
        }

        [TestMethod]
        public void When_HandleAcceptGame_Then_Success()
        {
            var acceptGameRequest = new GameAcceptedRequestObject
            {
                SessionName = this.sessionName,
                PlayerName = this.requestPlayer,
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
                It.Is<object>(o => ((GameCreatedNotificationObject)o).SessionName == this.session.Name),
                It.Is<string>(x => x == this.session.Player1.Information.UserName),
                It.Is<string>(x => x == this.session.Player2.Information.UserName)));
            this.playerSetupMock.VerifyAll();

            Assert.IsTrue(canHandle);
            Assert.IsTrue(this.session.Player1.PendingToMove);
            Assert.IsFalse(this.session.Player2.PendingToMove);
        }
    }
}
