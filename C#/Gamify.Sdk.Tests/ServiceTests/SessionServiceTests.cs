using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;
using Gamify.Sdk.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace Gamify.Sdk.Tests.ServiceTests
{
    [TestClass]
    public class SessionServiceTests
    {
        private ISessionService sessionService;

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void When_OpenSessionWithBothPlayers_Then_Success()
        {
            var playerService = Mock.Of<IPlayerService>();
            var sessionHistoryService = Mock.Of<ISessionHistoryService<TestMoveObject, TestResponseObject>>();
            var sessionRepository = new TestRepository<GameSession>();
            var sessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();

            this.sessionService = new SessionService(playerService, sessionHistoryService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = new GamePlayer
                {
                    UserName = "player1",
                    Name = "Player 1"
                }
            };
            var sessionPlayer2 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = new GamePlayer
                {
                    UserName = "player2",
                    Name = "Player 2"
                }
            };
            var newSession = this.sessionService.Open(sessionPlayer1, sessionPlayer2);

            Assert.IsNotNull(newSession);
            Assert.AreEqual("player1-vs-player2", newSession.Name);
            Assert.AreEqual(SessionState.Active, newSession.State);
            Assert.IsTrue(newSession.HasPlayer("player1"));
            Assert.IsTrue(newSession.HasPlayer("player2"));
            Assert.AreEqual("player1", newSession.GetVersusPlayer("player2").Information.UserName);
            Assert.IsTrue(newSession.Player1.IsReady);
            Assert.IsTrue(newSession.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession.Player1.SessionName);
            Assert.IsTrue(newSession.Player2.IsReady);
            Assert.IsFalse(newSession.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession.Player2.SessionName);
        }

        [TestMethod]
        public void When_OpenSessionWithOnePlayer_Then_Success()
        {
            var sessionHistoryService = Mock.Of<ISessionHistoryService<TestMoveObject, TestResponseObject>>();
            var gamePlayer = new GamePlayer { UserName = "player2", Name = "Player 2" };
            var sessionPlayer2 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = gamePlayer
            };
            var playerServiceMock = new Mock<IPlayerService>();

            playerServiceMock
                .Setup(s => s.GetRandom(It.Is<string>(n => n == "player1")))
                .Returns(gamePlayer);

            var playerService = playerServiceMock.Object;
            var sessionRepository = new TestRepository<GameSession>();

            var sessionPlayerFactoryMock = new Mock<ISessionPlayerFactory>();

            sessionPlayerFactoryMock
                .Setup(f => f.Create(It.Is<IGamePlayer>(p => p.UserName == gamePlayer.UserName), It.IsAny<ISessionHistoryService>()))
                .Returns(sessionPlayer2);

            var sessionPlayerFactory = sessionPlayerFactoryMock.Object;

            this.sessionService = new SessionService(playerService, sessionHistoryService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = new GamePlayer
                {
                    UserName = "player1",
                    Name = "Player 1"
                }
            };
            var newSession = this.sessionService.Open(sessionPlayer1);

            playerServiceMock.VerifyAll();
            sessionPlayerFactoryMock.VerifyAll();

            Assert.IsNotNull(newSession);
            Assert.AreEqual("player1-vs-player2", newSession.Name);
            Assert.AreEqual(SessionState.Active, newSession.State);
            Assert.IsTrue(newSession.HasPlayer("player1"));
            Assert.IsTrue(newSession.HasPlayer("player2"));
            Assert.AreEqual("player1", newSession.GetVersusPlayer("player2").Information.UserName);
            Assert.IsTrue(newSession.Player1.IsReady);
            Assert.IsTrue(newSession.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession.Player1.SessionName);
            Assert.IsTrue(newSession.Player2.IsReady);
            Assert.IsFalse(newSession.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession.Player2.SessionName);
        }

        [TestMethod]
        public void When_AbandonSession_Then_Success()
        {
            var playerService = Mock.Of<IPlayerService>();
            var sessionHistoryService = Mock.Of<ISessionHistoryService<TestMoveObject, TestResponseObject>>();
            var sessionRepository = new TestRepository<GameSession>();
            var sessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();

            this.sessionService = new SessionService(playerService, sessionHistoryService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = new GamePlayer
                {
                    UserName = "player1",
                    Name = "Player 1"
                }
            };
            var sessionPlayer2 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = new GamePlayer
                {
                    UserName = "player2",
                    Name = "Player 2"
                }
            };
            var newSession = this.sessionService.Open(sessionPlayer1, sessionPlayer2);

            this.sessionService.Abandon(newSession.Name);

            var abandonedSession = this.sessionService.GetByName(newSession.Name);

            Assert.IsNotNull(abandonedSession);
            Assert.AreEqual(SessionState.Abandoned, abandonedSession.State);
            Assert.AreEqual("player1-vs-player2", abandonedSession.Name);
            Assert.IsTrue(abandonedSession.HasPlayer("player1"));
            Assert.IsTrue(abandonedSession.HasPlayer("player2"));
            Assert.AreEqual("player1", abandonedSession.GetVersusPlayer("player2").Information.UserName);
            Assert.IsTrue(abandonedSession.Player1.IsReady);
            Assert.IsTrue(abandonedSession.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", abandonedSession.Player1.SessionName);
            Assert.IsTrue(abandonedSession.Player2.IsReady);
            Assert.IsFalse(abandonedSession.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", abandonedSession.Player2.SessionName);
        }

        [TestMethod]
        public void When_FinishSession_Then_Success()
        {
            var playerService = Mock.Of<IPlayerService>();
            var sessionHistoryService = Mock.Of<ISessionHistoryService<TestMoveObject, TestResponseObject>>();
            var sessionRepository = new TestRepository<GameSession>();
            var sessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();

            this.sessionService = new SessionService(playerService, sessionHistoryService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = new GamePlayer
                {
                    UserName = "player1",
                    Name = "Player 1"
                }
            };
            var sessionPlayer2 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = new GamePlayer
                {
                    UserName = "player2",
                    Name = "Player 2"
                }
            };
            var newSession = this.sessionService.Open(sessionPlayer1, sessionPlayer2);

            this.sessionService.Finish(newSession.Name);

            var abandonedSession = this.sessionService.GetByName(newSession.Name);

            Assert.IsNotNull(abandonedSession);
            Assert.AreEqual(SessionState.Finished, abandonedSession.State);
            Assert.AreEqual("player1-vs-player2", abandonedSession.Name);
            Assert.IsTrue(abandonedSession.HasPlayer("player1"));
            Assert.IsTrue(abandonedSession.HasPlayer("player2"));
            Assert.AreEqual("player1", abandonedSession.GetVersusPlayer("player2").Information.UserName);
            Assert.IsTrue(abandonedSession.Player1.IsReady);
            Assert.IsTrue(abandonedSession.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", abandonedSession.Player1.SessionName);
            Assert.IsTrue(abandonedSession.Player2.IsReady);
            Assert.IsFalse(abandonedSession.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", abandonedSession.Player2.SessionName);
        }

        [TestMethod]
        public void When_GetSessionsByPlayer_Then_Success()
        {
            var playerService = Mock.Of<IPlayerService>();
            var sessionHistoryService = Mock.Of<ISessionHistoryService<TestMoveObject, TestResponseObject>>();
            var sessionRepository = new TestRepository<GameSession>();
            var sessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();

            this.sessionService = new SessionService(playerService, sessionHistoryService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1_1 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = new GamePlayer
                {
                    UserName = "player1",
                    Name = "Player 1"
                }
            };
            var sessionPlayer1_3 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = new GamePlayer
                {
                    UserName = "player1",
                    Name = "Player 1"
                }
            };
            var sessionPlayer1_4 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = new GamePlayer
                {
                    UserName = "player1",
                    Name = "Player 1"
                }
            };
            var sessionPlayer2 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = new GamePlayer
                {
                    UserName = "player2",
                    Name = "Player 2"
                }
            };
            var sessionPlayer3 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = new GamePlayer
                {
                    UserName = "player3",
                    Name = "Player 3"
                }
            };
            var sessionPlayer4 = new TestSessionPlayer(sessionHistoryService)
            {
                Information = new GamePlayer
                {
                    UserName = "player4",
                    Name = "Player 4"
                }
            };
            var newSession1 = this.sessionService.Open(sessionPlayer1_1, sessionPlayer2);
            var newSession2 = this.sessionService.Open(sessionPlayer1_3, sessionPlayer3);
            var newSession3 = this.sessionService.Open(sessionPlayer1_4, sessionPlayer4);

            var newSessions = this.sessionService.GetAllByPlayer("player1");

            Assert.IsNotNull(newSessions);
            Assert.AreEqual(3, newSessions.Count());

            Assert.IsNotNull(newSession1);
            Assert.AreEqual("player1-vs-player2", newSession1.Name);
            Assert.AreEqual(SessionState.Active, newSession1.State);
            Assert.IsTrue(newSession1.HasPlayer("player1"));
            Assert.IsTrue(newSession1.HasPlayer("player2"));
            Assert.AreEqual("player1", newSession1.GetVersusPlayer("player2").Information.UserName);
            Assert.IsTrue(newSession1.Player1.IsReady);
            Assert.IsTrue(newSession1.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession1.Player1.SessionName);
            Assert.IsTrue(newSession1.Player2.IsReady);
            Assert.IsFalse(newSession1.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession1.Player2.SessionName);

            Assert.IsNotNull(newSession2);
            Assert.AreEqual("player1-vs-player3", newSession2.Name);
            Assert.AreEqual(SessionState.Active, newSession2.State);
            Assert.IsTrue(newSession2.HasPlayer("player1"));
            Assert.IsTrue(newSession2.HasPlayer("player3"));
            Assert.AreEqual("player1", newSession2.GetVersusPlayer("player3").Information.UserName);
            Assert.IsTrue(newSession2.Player1.IsReady);
            Assert.IsTrue(newSession2.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player3", newSession2.Player1.SessionName);
            Assert.IsTrue(newSession2.Player2.IsReady);
            Assert.IsFalse(newSession2.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player3", newSession2.Player2.SessionName);

            Assert.IsNotNull(newSession3);
            Assert.AreEqual("player1-vs-player4", newSession3.Name);
            Assert.AreEqual(SessionState.Active, newSession3.State);
            Assert.IsTrue(newSession3.HasPlayer("player1"));
            Assert.IsTrue(newSession3.HasPlayer("player4"));
            Assert.AreEqual("player1", newSession3.GetVersusPlayer("player4").Information.UserName);
            Assert.IsTrue(newSession3.Player1.IsReady);
            Assert.IsTrue(newSession3.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player4", newSession3.Player1.SessionName);
            Assert.IsTrue(newSession3.Player2.IsReady);
            Assert.IsFalse(newSession3.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player4", newSession3.Player2.SessionName);
        }
    }
}
