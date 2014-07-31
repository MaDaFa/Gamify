using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Interfaces;
using Gamify.Sdk.PluginComponents;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThinkUp.Sdk;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Data.Entities;
using ThinkUp.Sdk.Interfaces;
using ThinkUp.Sdk.Plugins.PluginComponents;
using ThinkUp.Sdk.Services;

namespace Gamify.Sdk.UnitTests.PluginComponentTests
{
    [TestClass]
    public class GameCreationPluginComponentTests
    {
        private readonly string sessionName = "player1-vs-player2";

        private ISerializer serializer;
        private IUser player1;
        private IUser player2;
        private SessionGamePlayer sessionPlayer1;
        private SessionGamePlayer sessionPlayer2;
        private IGameSession session;
        private Mock<IUserService> playerServiceMock;
        private Mock<ISessionService> sessionServiceMock;
        private Mock<INotificationService> notificationServiceMock;
        private Mock<ISessionPlayerFactory> playerFactoryMock;
        private Mock<ISessionPlayerSetup> playerSetupMock;
        private Mock<IGameInviteDecorator> inviteDecoratorMock;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();

            this.player1 = new User
            {
                DisplayName = "Player 1",
                Name = "player1"
            };
            this.player2 = new User
            {
                DisplayName = "Player 2",
                Name = "player2"
            };
            this.sessionPlayer1 = new TestSessionPlayer()
            {
                Information = this.player1,
                SessionName = this.sessionName
            };
            this.sessionPlayer2 = new TestSessionPlayer()
            {
                Information = this.player2,
                SessionName = this.sessionName
            };

            this.session = new GameSession(this.sessionPlayer1, this.sessionPlayer2);

            this.playerServiceMock = new Mock<IUserService>();
            this.sessionServiceMock = new Mock<ISessionService>();
            this.notificationServiceMock = new Mock<INotificationService>();
            this.playerFactoryMock = new Mock<ISessionPlayerFactory>();
            this.playerSetupMock = new Mock<ISessionPlayerSetup>();
            this.inviteDecoratorMock = new Mock<IGameInviteDecorator>();
        }

        [TestMethod]
        public void UT_When_HandleCreateGame_Then_Success()
        {
            this.playerServiceMock
               .Setup(s => s.GetByName(It.Is<string>(x => x == this.player1.DisplayName)))
               .Returns(this.player1)
               .Verifiable();
            this.playerServiceMock
               .Setup(s => s.GetByName(It.Is<string>(x => x == this.player2.DisplayName)))
               .Returns(this.player2)
               .Verifiable();
            this.playerFactoryMock
                .Setup(f => f.Create(It.Is<IUser>(p => p == this.player1)))
                .Returns(this.sessionPlayer1)
                .Verifiable();
            this.playerFactoryMock
                .Setup(f => f.Create(It.Is<IUser>(p => p == this.player2)))
                .Returns(this.sessionPlayer2)
                .Verifiable();
            this.playerSetupMock.Setup(s => s.GetPlayerReady(It.Is<CreateGameClientMessage>(x => x.UserName == this.player1.DisplayName
                && x.InvitedUserName == this.player2.DisplayName),
                It.Is<TestSessionPlayer>(p => p == this.sessionPlayer1)))
                .Verifiable();
            this.sessionServiceMock.Setup(s => s.Create(It.Is<SessionGamePlayer>(p => p == this.sessionPlayer1),
                It.Is<SessionGamePlayer>(p => p == this.sessionPlayer2)))
                .Returns(this.session)
                .Verifiable();
            this.inviteDecoratorMock
                .Setup(d => d.Decorate(It.Is<GameInviteReceivedServerMessage>(n => n.SessionName == this.sessionName && n.Player1Name == this.player1.Name),
                    It.Is<IGameSession>(s => s == this.session)))
                .Verifiable();

            var createGameClientMessage = new CreateGameClientMessage
            {
                UserName = this.player1.DisplayName,
                InvitedUserName = this.player2.DisplayName,
                AdditionalInformation = "Test"
            };
            var clientContract = new ClientContract
            {
                Type = GamifyClientMessageType.CreateGame,
                Sender = this.player1.DisplayName,
                SerializedClientMessage = this.serializer.Serialize(createGameClientMessage)
            };

            var gameCreationPluginComponent = this.GetGameCreationPluginComponent();
            var canHandle = gameCreationPluginComponent.CanHandleClientMessage(clientContract);

            gameCreationPluginComponent.HandleClientMessage(clientContract);

            this.playerServiceMock.VerifyAll();
            this.sessionServiceMock.VerifyAll();
            this.playerFactoryMock.VerifyAll();
            this.playerSetupMock.VerifyAll();
            this.inviteDecoratorMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<int>(t => t == GamifyServerMessageType.GameInviteReceived),
                    It.Is<object>(o => ((GameInviteReceivedServerMessage)o).Player1Name == this.player1.Name
                        && ((GameInviteReceivedServerMessage)o).SessionName == this.sessionName),
                    It.Is<string>(x => x == this.player2.Name)));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void UT_When_HandleAcceptGame_Then_Success()
        {
            this.sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == this.session.Name)))
                .Returns(this.session)
                .Verifiable();
            this.sessionServiceMock
                .Setup(s => s.Start(It.Is<IGameSession>(x => x.Name == this.session.Name)))
                .Verifiable();
            this.playerSetupMock.Setup(s => s.GetPlayerReady(It.Is<AcceptGameClientMessage>(x => x.SessionName == this.sessionName),
                It.Is<TestSessionPlayer>(p => p.Information.Name == "player2")))
                .Verifiable();

            var acceptGameClientMessage = new AcceptGameClientMessage
            {
                SessionName = this.sessionName,
                UserName = this.player2.Name,
                AdditionalInformation = "Test"
            };
            var clientContract = new ClientContract
            {
                Type = GamifyClientMessageType.AcceptGame,
                Sender = this.player2.Name,
                SerializedClientMessage = this.serializer.Serialize(acceptGameClientMessage)
            };

            var gameCreationPluginComponent = this.GetGameCreationPluginComponent();
            var canHandle = gameCreationPluginComponent.CanHandleClientMessage(clientContract);

            gameCreationPluginComponent.HandleClientMessage(clientContract);

            this.sessionServiceMock.VerifyAll();
            this.playerSetupMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<int>(t => t == GamifyServerMessageType.GameCreated),
                It.Is<object>(o => ((GameCreatedServerMessage)o).SessionName == this.session.Name),
                It.Is<string>(x => x == this.session.Player1Name),
                It.Is<string>(x => x == this.session.Player2Name)));

            Assert.IsTrue(canHandle);
            Assert.IsTrue(this.session.Player1.PendingToMove);
            Assert.IsFalse(this.session.Player2.PendingToMove);
        }

        [TestMethod]
        public void UT_When_HandleRejectGame_Then_Success()
        {
            this.sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == this.session.Name)))
                .Returns(this.session)
                .Verifiable();

            var rejectGameClientMessage = new RejectGameClientMessage
            {
                SessionName = this.sessionName,
                UserName = this.player2.Name
            };
            var clientContract = new ClientContract
            {
                Type = GamifyClientMessageType.RejectGame,
                Sender = this.player2.Name,
                SerializedClientMessage = this.serializer.Serialize(rejectGameClientMessage)
            };

            var gameCreationPluginComponent = this.GetGameCreationPluginComponent();
            var canHandle = gameCreationPluginComponent.CanHandleClientMessage(clientContract);

            gameCreationPluginComponent.HandleClientMessage(clientContract);

            this.sessionServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<int>(t => t == GamifyServerMessageType.GameRejected),
                It.Is<object>(o => ((GameRejectedServerMessage)o).SessionName == this.session.Name
                    && ((GameRejectedServerMessage)o).Player1Name == this.session.Player1Name
                    && ((GameRejectedServerMessage)o).Player2Name == this.session.Player2Name),
                It.Is<string>(x => x == this.session.Player1Name)));

            Assert.IsTrue(canHandle);
        }

        private IPluginComponent GetGameCreationPluginComponent()
        {
            return new GameCreationPluginComponent(this.playerServiceMock.Object, this.sessionServiceMock.Object,
                this.notificationServiceMock.Object, this.playerFactoryMock.Object,
                this.playerSetupMock.Object, this.inviteDecoratorMock.Object, this.serializer);
        }
    }
}
