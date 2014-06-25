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
    public class CreateGameComponentTests
    {
        private readonly string sessionName = "player1-vs-player2";

        private ISerializer serializer;

        private IGamePlayer player1;
        private IGamePlayer player2;
        private IGameSession session;
        private Mock<IPlayerService> playerServiceMock;
        private Mock<ISessionService> sessionServiceMock;
        private Mock<INotificationService> notificationServiceMock;
        private Mock<ISessionPlayerFactory> playerFactoryMock;
        private Mock<ISessionPlayerSetup> playerSetupMock;
        private Mock<IGameInviteDecorator> inviteDecoratorMock;

        private IGameComponent createGameComponent;

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

            var sessionPlayer1 = new TestSessionPlayer()
            {
                Information = this.player1,
                SessionName = this.sessionName
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                Information = this.player2,
                SessionName = this.sessionName
            };

            this.session = new GameSession(sessionPlayer1, sessionPlayer2);

            this.playerServiceMock = new Mock<IPlayerService>();
            this.playerServiceMock
               .Setup(s => s.GetByName(It.Is<string>(x => x == this.player1.DisplayName)))
               .Returns(this.player1)
               .Verifiable();
            this.playerServiceMock
               .Setup(s => s.GetByName(It.Is<string>(x => x == this.player2.DisplayName)))
               .Returns(this.player2)
               .Verifiable();

            this.sessionServiceMock = new Mock<ISessionService>();
            this.sessionServiceMock.Setup(s => s.Create(It.Is<SessionGamePlayer>(p => p == sessionPlayer1),
                It.Is<SessionGamePlayer>(p => p == sessionPlayer2)))
                .Returns(this.session)
                .Verifiable();

            this.notificationServiceMock = new Mock<INotificationService>();

            this.playerFactoryMock = new Mock<ISessionPlayerFactory>();
            this.playerFactoryMock
                .Setup(f => f.Create(It.Is<IGamePlayer>(p => p == this.player1)))
                .Returns(sessionPlayer1)
                .Verifiable();
            this.playerFactoryMock
                .Setup(f => f.Create(It.Is<IGamePlayer>(p => p == this.player2)))
                .Returns(sessionPlayer2)
                .Verifiable();

            this.playerSetupMock = new Mock<ISessionPlayerSetup>();
            this.playerSetupMock.Setup(s => s.GetPlayerReady(It.Is<CreateGameRequestObject>(x => x.PlayerName == this.player1.DisplayName
                && x.InvitedPlayerName == this.player2.DisplayName),
                It.Is<TestSessionPlayer>(p => p == sessionPlayer1)))
                .Verifiable();

            this.inviteDecoratorMock = new Mock<IGameInviteDecorator>();
            this.inviteDecoratorMock
                .Setup(d => d.Decorate(It.Is<GameInviteNotificationObject>(n => n.SessionName == this.sessionName && n.Player1Name == this.player1.Name),
                    It.Is<IGameSession>(s => s == this.session)))
                .Verifiable();

            this.createGameComponent = new CreateGameComponent(this.playerServiceMock.Object, this.sessionServiceMock.Object,
                this.notificationServiceMock.Object, this.playerFactoryMock.Object,
                this.playerSetupMock.Object, this.inviteDecoratorMock.Object, this.serializer);
        }

        [TestMethod]
        public void UT_When_HandleCreateGame_Then_Success()
        {
            var connectPlayerRequest = new CreateGameRequestObject
            {
                PlayerName = this.player1.DisplayName,
                InvitedPlayerName = this.player2.DisplayName,
                AdditionalInformation = "Test"
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.CreateGame,
                SerializedRequestObject = this.serializer.Serialize(connectPlayerRequest)
            };

            var canHandle = this.createGameComponent.CanHandleRequest(gameRequest);

            this.createGameComponent.HandleRequest(gameRequest);

            this.playerServiceMock.VerifyAll();
            this.sessionServiceMock.VerifyAll();
            this.playerFactoryMock.VerifyAll();
            this.playerSetupMock.VerifyAll();
            this.inviteDecoratorMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<GameNotificationType>(t => t == GameNotificationType.GameInvite),
                    It.Is<object>(o => ((GameInviteNotificationObject)o).Player1Name == this.player1.Name
                        && ((GameInviteNotificationObject)o).SessionName == this.sessionName),
                    It.Is<string>(x => x == this.player2.Name)));

            Assert.IsTrue(canHandle);
        }
    }
}
