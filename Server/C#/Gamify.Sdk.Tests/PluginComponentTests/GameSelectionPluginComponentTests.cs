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
using System.Collections.Generic;
using System.Linq;
using ThinkUp.Sdk;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Data.Entities;
using ThinkUp.Sdk.Plugins.PluginComponents;
using ThinkUp.Sdk.Services;

namespace Gamify.Sdk.UnitTests.PluginComponentTests
{
    [TestClass]
    public class GameSelectionPluginComponentTests
    {
        private readonly string requestPlayer = "player2";
        private readonly string session1Name = "player1-vs-player2";
        private readonly string session2Name = "player1-vs-player3";

        private ISerializer serializer;
        private IList<IGameSession> sessions;
        private Mock<ISessionService> sessionServiceMock;
        private Mock<ISessionHistoryService<TestMoveObject, TestResponseObject>>  sessionHistoryServiceMock;
        private Mock<INotificationService> notificationServiceMock;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();

            var player1s1 = new TestSessionPlayer()
            {
                SessionName = this.session1Name,
                PendingToMove = false,
                Information = new User
                {
                    DisplayName = "Player 1",
                    Name = "player1"
                }
            };
            var player2s1 = new TestSessionPlayer()
            {
                SessionName = this.session1Name,
                PendingToMove = true,
                Information = new User
                {
                    DisplayName = "Player 2",
                    Name = "player2"
                }
            };
            var player1s2 = new TestSessionPlayer()
            {
                SessionName = this.session2Name,
                PendingToMove = false,
                Information = new User
                {
                    DisplayName = "Player 1",
                    Name = "player1"
                }
            };
            var player3s2 = new TestSessionPlayer()
            {
                SessionName = this.session2Name,
                PendingToMove = false,
                Information = new User
                {
                    DisplayName = "Player 3",
                    Name = "player3"
                }
            };

            var session1 = new GameSession(player1s1, player2s1);
            var session2 = new GameSession(player1s2, player3s2);

            this.sessions = new List<IGameSession>();
            this.sessions.Add(session1);
            this.sessions.Add(session2);

            this.sessionServiceMock = new Mock<ISessionService>();
            this.sessionHistoryServiceMock = new Mock<ISessionHistoryService<TestMoveObject, TestResponseObject>>();
            this.notificationServiceMock = new Mock<INotificationService>();
        }

        [TestMethod]
        public void UT_When_HandleOpenGame_Then_Success()
        {
            var session1 = this.sessions.First(s => s.Name == this.session1Name);
            var session1Player1Name = session1.Player1Name;
            var session1Player2Name = session1.Player2Name;
            var player1SessionHistory = new SessionHistory<TestMoveObject, TestResponseObject>(this.session1Name, session1Player1Name);
            var player2SessionHistory = new SessionHistory<TestMoveObject, TestResponseObject>(this.session1Name, session1Player2Name);

            this.sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == this.session1Name)))
                .Returns(session1)
                .Verifiable();
            this.sessionHistoryServiceMock
                .Setup(s => s.GetBySessionPlayer(It.Is<string>(x => x == this.session1Name), It.Is<string>(x => x == session1Player1Name)))
                .Returns(player1SessionHistory)
                .Verifiable();
            this.sessionHistoryServiceMock
                .Setup(s => s.GetBySessionPlayer(It.Is<string>(x => x == this.session1Name), It.Is<string>(x => x == session1Player2Name)))
                .Returns(player2SessionHistory)
                .Verifiable();

            var openGameClientMessage = new OpenGameClientMessage
            {
                SessionName = this.session1Name,
                UserName = this.requestPlayer
            };
            var clientContract = new ClientContract
            {
                Type = GamifyClientMessageType.OpenGame,
                Sender = this.requestPlayer,
                SerializedClientMessage = this.serializer.Serialize(openGameClientMessage)
            };

            var gameSelectionPluginComponent = this.GetGameSelectionPluginComponent();
            var canHandle = gameSelectionPluginComponent.CanHandleClientMessage(clientContract);

            gameSelectionPluginComponent.HandleClientMessage(clientContract);

            this.sessionServiceMock.VerifyAll();
            this.sessionHistoryServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<int>(t => t == GamifyServerMessageType.GameInformationReceived),
                It.Is<object>(o => ((GameInformationReceivedServerMessage)o).SessionName == this.session1Name),
                It.Is<string>(x => x == this.requestPlayer)));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void UT_When_HandleGetActiveGames_Then_Success()
        {
            this.sessionServiceMock
                .Setup(s => s.GetActives(It.Is<string>(x => x == this.requestPlayer)))
                .Returns(this.sessions)
                .Verifiable();

            var getActiveGamesClientMessage = new GetActiveGamesClientMessage
            {
                UserName = this.requestPlayer
            };
            var clientContract = new ClientContract
            {
                Type = GamifyClientMessageType.GetActiveGames,
                Sender = this.requestPlayer,
                SerializedClientMessage = this.serializer.Serialize(getActiveGamesClientMessage)
            };

            var gameSelectionPluginComponent = this.GetGameSelectionPluginComponent();
            var canHandle = gameSelectionPluginComponent.CanHandleClientMessage(clientContract);

            gameSelectionPluginComponent.HandleClientMessage(clientContract);

            this.sessionServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<int>(t => t == GamifyServerMessageType.ActiveGamesList),
                It.Is<object>(o => (((ActiveGamesListServerMessage)o).ActiveGamesCount == 2)
                    && ((ActiveGamesListServerMessage)o).PlayerName == this.requestPlayer),
                It.Is<string>(x => x == this.requestPlayer)));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void UT_When_HandleGetFinishedGames_Then_Success()
        {
            this.sessionServiceMock
                .Setup(s => s.GetFinished(It.Is<string>(x => x == this.requestPlayer)))
                .Returns(this.sessions)
                .Verifiable();

            var getFinishedGamesClientMessage = new GetFinishedGamesClientMessage
            {
                UserName = this.requestPlayer
            };
            var clientContract = new ClientContract
            {
                Type = GamifyClientMessageType.GetFinishedGames,
                Sender = this.requestPlayer, 
                SerializedClientMessage = this.serializer.Serialize(getFinishedGamesClientMessage)
            };

            var gameSelectionPluginComponent = this.GetGameSelectionPluginComponent();
            var canHandle = gameSelectionPluginComponent.CanHandleClientMessage(clientContract);

            gameSelectionPluginComponent.HandleClientMessage(clientContract);

            this.sessionServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<int>(t => t == GamifyServerMessageType.FinishedGamesList),
                It.Is<object>(o => (((FinishedGamesListServerMessage)o).FinishedGamesCount == 2)
                    && ((FinishedGamesListServerMessage)o).PlayerName == this.requestPlayer),
                It.Is<string>(x => x == this.requestPlayer)));

            Assert.IsTrue(canHandle);
        }
        [TestMethod]
        public void UT_When_HandleGetPendingGames_Then_Success()
        {
            this.sessionServiceMock
                .Setup(s => s.GetPendings(It.Is<string>(x => x == this.requestPlayer)))
                .Returns(this.sessions)
                .Verifiable();

            var getPendingGamesClientMessage = new GetPendingGamesClientMessage
            {
                UserName = this.requestPlayer
            };
            var clientContract = new ClientContract
            {
                Type = GamifyClientMessageType.GetPendingGames,
                Sender = this.requestPlayer,
                SerializedClientMessage = this.serializer.Serialize(getPendingGamesClientMessage)
            };

            var gameSelectionPluginComponent = this.GetGameSelectionPluginComponent();
            var canHandle = gameSelectionPluginComponent.CanHandleClientMessage(clientContract);

            gameSelectionPluginComponent.HandleClientMessage(clientContract);

            this.sessionServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<int>(t => t == GamifyServerMessageType.PendingGamesList),
                It.Is<object>(o => (((PendingGamesListServerMessage)o).PendingGamesCount == 2)
                    && ((PendingGamesListServerMessage)o).PlayerName == this.requestPlayer),
                It.Is<string>(x => x == this.requestPlayer)));

            Assert.IsTrue(canHandle);
        }

        private IPluginComponent GetGameSelectionPluginComponent()
        {
            var playerHistoryItemFactory = Mock.Of<IPlayerHistoryItemFactory<TestMoveObject, TestResponseObject>>();

            return new GameSelectionPluginComponent<TestMoveObject, TestResponseObject>(this.sessionServiceMock.Object,
                sessionHistoryServiceMock.Object, this.notificationServiceMock.Object, playerHistoryItemFactory, this.serializer);
        }
    }
}
