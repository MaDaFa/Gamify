using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace Gamify.Sdk.UnitTests.ServiceTests
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
        public void UT_When_CreateSessionWithBothPlayers_Then_Success()
        {
            var playerService = Mock.Of<IPlayerService>();
            var sessionRepository = new TestRepository<GameSession>();
            var sessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();

            this.sessionService = new SessionService(playerService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player2",
                    DisplayName = "Player 2"
                }
            };
            var newSession = this.sessionService.Create(sessionPlayer1, sessionPlayer2);

            Assert.IsNotNull(newSession);
            Assert.AreEqual("player1-vs-player2", newSession.Name);
            Assert.AreEqual(SessionState.Pending, newSession.State);
            Assert.IsTrue(newSession.HasPlayer("player1"));
            Assert.IsTrue(newSession.HasPlayer("player2"));
            Assert.AreEqual("player1", newSession.GetVersusPlayer("player2").Information.Name);
            Assert.IsTrue(newSession.Player1.IsReady);
            Assert.IsTrue(newSession.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession.Player1.SessionName);
            Assert.IsTrue(newSession.Player2.IsReady);
            Assert.IsFalse(newSession.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession.Player2.SessionName);
        }

        [TestMethod]
        public void UT_When_CreateSessionWithOnePlayer_Then_Success()
        {
            var gamePlayer = new GamePlayer { Name = "player2", DisplayName = "Player 2" };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                Information = gamePlayer
            };
            var playerServiceMock = new Mock<IPlayerService>();

            playerServiceMock
                .Setup(s => s.GetRandom(It.Is<string>(n => n == "player1")))
                .Returns(gamePlayer)
                .Verifiable();

            var playerService = playerServiceMock.Object;
            var sessionRepository = new TestRepository<GameSession>();

            var sessionPlayerFactoryMock = new Mock<ISessionPlayerFactory>();

            sessionPlayerFactoryMock
                .Setup(f => f.Create(It.Is<IGamePlayer>(p => p.Name == gamePlayer.Name)))
                .Returns(sessionPlayer2)
                .Verifiable();

            var sessionPlayerFactory = sessionPlayerFactoryMock.Object;

            this.sessionService = new SessionService(playerService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var newSession = this.sessionService.Create(sessionPlayer1);

            playerServiceMock.VerifyAll();
            sessionPlayerFactoryMock.VerifyAll();

            Assert.IsNotNull(newSession);
            Assert.AreEqual("player1-vs-player2", newSession.Name);
            Assert.AreEqual(SessionState.Pending, newSession.State);
            Assert.IsTrue(newSession.HasPlayer("player1"));
            Assert.IsTrue(newSession.HasPlayer("player2"));
            Assert.AreEqual("player1", newSession.GetVersusPlayer("player2").Information.Name);
            Assert.IsTrue(newSession.Player1.IsReady);
            Assert.IsTrue(newSession.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession.Player1.SessionName);
            Assert.IsTrue(newSession.Player2.IsReady);
            Assert.IsFalse(newSession.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession.Player2.SessionName);
        }

        [TestMethod]
        public void UT_When_StartSession_Then_Success()
        {
            var playerService = Mock.Of<IPlayerService>();
            var sessionRepository = new TestRepository<GameSession>();
            var sessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();

            this.sessionService = new SessionService(playerService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player2",
                    DisplayName = "Player 2"
                }
            };
            var newSession = this.sessionService.Create(sessionPlayer1, sessionPlayer2);

            this.sessionService.Start(newSession);

            var startedSession = this.sessionService.GetByName(newSession.Name);

            Assert.IsNotNull(startedSession);
            Assert.AreEqual("player1-vs-player2", startedSession.Name);
            Assert.AreEqual(SessionState.Active, startedSession.State);
            Assert.IsTrue(startedSession.HasPlayer("player1"));
            Assert.IsTrue(startedSession.HasPlayer("player2"));
            Assert.AreEqual("player1", startedSession.GetVersusPlayer("player2").Information.Name);
            Assert.IsTrue(startedSession.Player1.IsReady);
            Assert.IsTrue(startedSession.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", startedSession.Player1.SessionName);
            Assert.IsTrue(startedSession.Player2.IsReady);
            Assert.IsFalse(startedSession.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", startedSession.Player2.SessionName);
        }

        [TestMethod]
        public void UT_When_AbandonSession_Then_Success()
        {
            var playerService = Mock.Of<IPlayerService>();
            var sessionRepository = new TestRepository<GameSession>();
            var sessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();

            this.sessionService = new SessionService(playerService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player2",
                    DisplayName = "Player 2"
                }
            };
            var newSession = this.sessionService.Create(sessionPlayer1, sessionPlayer2);

            this.sessionService.Start(newSession);

            var startedSession = this.sessionService.GetByName(newSession.Name);

            this.sessionService.Abandon(startedSession.Name);

            var abandonedSession = this.sessionService.GetByName(newSession.Name);

            Assert.IsNotNull(abandonedSession);
            Assert.AreEqual(SessionState.Abandoned, abandonedSession.State);
            Assert.AreEqual("player1-vs-player2", abandonedSession.Name);
            Assert.IsTrue(abandonedSession.HasPlayer("player1"));
            Assert.IsTrue(abandonedSession.HasPlayer("player2"));
            Assert.AreEqual("player1", abandonedSession.GetVersusPlayer("player2").Information.Name);
            Assert.IsTrue(abandonedSession.Player1.IsReady);
            Assert.IsTrue(abandonedSession.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", abandonedSession.Player1.SessionName);
            Assert.IsTrue(abandonedSession.Player2.IsReady);
            Assert.IsFalse(abandonedSession.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", abandonedSession.Player2.SessionName);
        }

        [TestMethod]
        public void UT_When_FinishSession_Then_Success()
        {
            var playerService = Mock.Of<IPlayerService>();
            var sessionRepository = new TestRepository<GameSession>();
            var sessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();

            this.sessionService = new SessionService(playerService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player2",
                    DisplayName = "Player 2"
                }
            };
            var newSession = this.sessionService.Create(sessionPlayer1, sessionPlayer2);

            this.sessionService.Start(newSession);

            var startedSession = this.sessionService.GetByName(newSession.Name);

            this.sessionService.Finish(startedSession.Name);

            var abandonedSession = this.sessionService.GetByName(newSession.Name);

            Assert.IsNotNull(abandonedSession);
            Assert.AreEqual(SessionState.Finished, abandonedSession.State);
            Assert.AreEqual("player1-vs-player2", abandonedSession.Name);
            Assert.IsTrue(abandonedSession.HasPlayer("player1"));
            Assert.IsTrue(abandonedSession.HasPlayer("player2"));
            Assert.AreEqual("player1", abandonedSession.GetVersusPlayer("player2").Information.Name);
            Assert.IsTrue(abandonedSession.Player1.IsReady);
            Assert.IsTrue(abandonedSession.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", abandonedSession.Player1.SessionName);
            Assert.IsTrue(abandonedSession.Player2.IsReady);
            Assert.IsFalse(abandonedSession.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", abandonedSession.Player2.SessionName);
        }

        [TestMethod]
        public void UT_When_GetSessionsByPlayer_Then_Success()
        {
            var playerService = Mock.Of<IPlayerService>();
            var sessionRepository = new TestRepository<GameSession>();
            var sessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();

            this.sessionService = new SessionService(playerService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1_1 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer1_3 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer1_4 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player2",
                    DisplayName = "Player 2"
                }
            };
            var sessionPlayer3 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player3",
                    DisplayName = "Player 3"
                }
            };
            var sessionPlayer4 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player4",
                    DisplayName = "Player 4"
                }
            };
            var newSession1 = this.sessionService.Create(sessionPlayer1_1, sessionPlayer2);
            var newSession2 = this.sessionService.Create(sessionPlayer1_3, sessionPlayer3);
            var newSession3 = this.sessionService.Create(sessionPlayer1_4, sessionPlayer4);

            var newSessions = this.sessionService.GetAllByPlayer("player1");

            Assert.IsNotNull(newSessions);
            Assert.AreEqual(3, newSessions.Count());

            Assert.IsNotNull(newSession1);
            Assert.AreEqual("player1-vs-player2", newSession1.Name);
            Assert.AreEqual(SessionState.Pending, newSession1.State);
            Assert.IsTrue(newSession1.HasPlayer("player1"));
            Assert.IsTrue(newSession1.HasPlayer("player2"));
            Assert.AreEqual("player1", newSession1.GetVersusPlayer("player2").Information.Name);
            Assert.IsTrue(newSession1.Player1.IsReady);
            Assert.IsTrue(newSession1.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession1.Player1.SessionName);
            Assert.IsTrue(newSession1.Player2.IsReady);
            Assert.IsFalse(newSession1.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession1.Player2.SessionName);

            Assert.IsNotNull(newSession2);
            Assert.AreEqual("player1-vs-player3", newSession2.Name);
            Assert.AreEqual(SessionState.Pending, newSession2.State);
            Assert.IsTrue(newSession2.HasPlayer("player1"));
            Assert.IsTrue(newSession2.HasPlayer("player3"));
            Assert.AreEqual("player1", newSession2.GetVersusPlayer("player3").Information.Name);
            Assert.IsTrue(newSession2.Player1.IsReady);
            Assert.IsTrue(newSession2.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player3", newSession2.Player1.SessionName);
            Assert.IsTrue(newSession2.Player2.IsReady);
            Assert.IsFalse(newSession2.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player3", newSession2.Player2.SessionName);

            Assert.IsNotNull(newSession3);
            Assert.AreEqual("player1-vs-player4", newSession3.Name);
            Assert.AreEqual(SessionState.Pending, newSession3.State);
            Assert.IsTrue(newSession3.HasPlayer("player1"));
            Assert.IsTrue(newSession3.HasPlayer("player4"));
            Assert.AreEqual("player1", newSession3.GetVersusPlayer("player4").Information.Name);
            Assert.IsTrue(newSession3.Player1.IsReady);
            Assert.IsTrue(newSession3.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player4", newSession3.Player1.SessionName);
            Assert.IsTrue(newSession3.Player2.IsReady);
            Assert.IsFalse(newSession3.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player4", newSession3.Player2.SessionName);
        }

        [TestMethod]
        public void UT_When_GetPendingSessions_Then_Success()
        {
            var playerService = Mock.Of<IPlayerService>();
            var sessionRepository = new TestRepository<GameSession>();
            var sessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();

            this.sessionService = new SessionService(playerService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1_1 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer1_3 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer1_4 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player2",
                    DisplayName = "Player 2"
                }
            };
            var sessionPlayer3 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player3",
                    DisplayName = "Player 3"
                }
            };
            var sessionPlayer4 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player4",
                    DisplayName = "Player 4"
                }
            };
            var newSession1 = this.sessionService.Create(sessionPlayer1_1, sessionPlayer2);
            var newSession2 = this.sessionService.Create(sessionPlayer1_3, sessionPlayer3);
            var newSession3 = this.sessionService.Create(sessionPlayer1_4, sessionPlayer4);

            this.sessionService.Start(newSession2);

            var startedSession2 = this.sessionService.GetByName(newSession2.Name);
            var pendingSessions = this.sessionService.GetPendings("player1");

            Assert.IsNotNull(pendingSessions);
            Assert.AreEqual(2, pendingSessions.Count());

            Assert.IsNotNull(newSession1);
            Assert.AreEqual("player1-vs-player2", newSession1.Name);
            Assert.AreEqual(SessionState.Pending, newSession1.State);
            Assert.IsTrue(newSession1.HasPlayer("player1"));
            Assert.IsTrue(newSession1.HasPlayer("player2"));
            Assert.AreEqual("player1", newSession1.GetVersusPlayer("player2").Information.Name);
            Assert.IsTrue(newSession1.Player1.IsReady);
            Assert.IsTrue(newSession1.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession1.Player1.SessionName);
            Assert.IsTrue(newSession1.Player2.IsReady);
            Assert.IsFalse(newSession1.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession1.Player2.SessionName);

            Assert.IsNotNull(startedSession2);
            Assert.AreEqual("player1-vs-player3", startedSession2.Name);
            Assert.AreEqual(SessionState.Active, startedSession2.State);
            Assert.IsTrue(startedSession2.HasPlayer("player1"));
            Assert.IsTrue(startedSession2.HasPlayer("player3"));
            Assert.AreEqual("player1", startedSession2.GetVersusPlayer("player3").Information.Name);
            Assert.IsTrue(startedSession2.Player1.IsReady);
            Assert.IsTrue(startedSession2.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player3", startedSession2.Player1.SessionName);
            Assert.IsTrue(startedSession2.Player2.IsReady);
            Assert.IsFalse(startedSession2.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player3", startedSession2.Player2.SessionName);

            Assert.IsNotNull(newSession3);
            Assert.AreEqual("player1-vs-player4", newSession3.Name);
            Assert.AreEqual(SessionState.Pending, newSession3.State);
            Assert.IsTrue(newSession3.HasPlayer("player1"));
            Assert.IsTrue(newSession3.HasPlayer("player4"));
            Assert.AreEqual("player1", newSession3.GetVersusPlayer("player4").Information.Name);
            Assert.IsTrue(newSession3.Player1.IsReady);
            Assert.IsTrue(newSession3.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player4", newSession3.Player1.SessionName);
            Assert.IsTrue(newSession3.Player2.IsReady);
            Assert.IsFalse(newSession3.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player4", newSession3.Player2.SessionName);
        }

        [TestMethod]
        public void UT_When_GetActiveSessions_Then_Success()
        {
            var playerService = Mock.Of<IPlayerService>();
            var sessionRepository = new TestRepository<GameSession>();
            var sessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();

            this.sessionService = new SessionService(playerService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1_1 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer1_3 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer1_4 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player2",
                    DisplayName = "Player 2"
                }
            };
            var sessionPlayer3 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player3",
                    DisplayName = "Player 3"
                }
            };
            var sessionPlayer4 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player4",
                    DisplayName = "Player 4"
                }
            };
            var newSession1 = this.sessionService.Create(sessionPlayer1_1, sessionPlayer2);
            var newSession2 = this.sessionService.Create(sessionPlayer1_3, sessionPlayer3);
            var newSession3 = this.sessionService.Create(sessionPlayer1_4, sessionPlayer4);

            this.sessionService.Start(newSession2);

            var startedSession2 = this.sessionService.GetByName(newSession2.Name);
            var activeSessions = this.sessionService.GetActives("player1");

            Assert.IsNotNull(activeSessions);
            Assert.AreEqual(1, activeSessions.Count());

            Assert.IsNotNull(newSession1);
            Assert.AreEqual("player1-vs-player2", newSession1.Name);
            Assert.AreEqual(SessionState.Pending, newSession1.State);
            Assert.IsTrue(newSession1.HasPlayer("player1"));
            Assert.IsTrue(newSession1.HasPlayer("player2"));
            Assert.AreEqual("player1", newSession1.GetVersusPlayer("player2").Information.Name);
            Assert.IsTrue(newSession1.Player1.IsReady);
            Assert.IsTrue(newSession1.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession1.Player1.SessionName);
            Assert.IsTrue(newSession1.Player2.IsReady);
            Assert.IsFalse(newSession1.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession1.Player2.SessionName);

            Assert.IsNotNull(startedSession2);
            Assert.AreEqual("player1-vs-player3", startedSession2.Name);
            Assert.AreEqual(SessionState.Active, startedSession2.State);
            Assert.IsTrue(startedSession2.HasPlayer("player1"));
            Assert.IsTrue(startedSession2.HasPlayer("player3"));
            Assert.AreEqual("player1", startedSession2.GetVersusPlayer("player3").Information.Name);
            Assert.IsTrue(startedSession2.Player1.IsReady);
            Assert.IsTrue(startedSession2.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player3", startedSession2.Player1.SessionName);
            Assert.IsTrue(startedSession2.Player2.IsReady);
            Assert.IsFalse(startedSession2.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player3", startedSession2.Player2.SessionName);

            Assert.IsNotNull(newSession3);
            Assert.AreEqual("player1-vs-player4", newSession3.Name);
            Assert.AreEqual(SessionState.Pending, newSession3.State);
            Assert.IsTrue(newSession3.HasPlayer("player1"));
            Assert.IsTrue(newSession3.HasPlayer("player4"));
            Assert.AreEqual("player1", newSession3.GetVersusPlayer("player4").Information.Name);
            Assert.IsTrue(newSession3.Player1.IsReady);
            Assert.IsTrue(newSession3.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player4", newSession3.Player1.SessionName);
            Assert.IsTrue(newSession3.Player2.IsReady);
            Assert.IsFalse(newSession3.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player4", newSession3.Player2.SessionName);
        }

        [TestMethod]
        public void UT_When_GetFinishedSessions_Then_Success()
        {
            var playerService = Mock.Of<IPlayerService>();
            var sessionRepository = new TestRepository<GameSession>();
            var sessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();

            this.sessionService = new SessionService(playerService, sessionRepository, sessionPlayerFactory);

            var sessionPlayer1_1 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer1_3 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer1_4 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player1",
                    DisplayName = "Player 1"
                }
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player2",
                    DisplayName = "Player 2"
                }
            };
            var sessionPlayer3 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player3",
                    DisplayName = "Player 3"
                }
            };
            var sessionPlayer4 = new TestSessionPlayer()
            {
                Information = new GamePlayer
                {
                    Name = "player4",
                    DisplayName = "Player 4"
                }
            };
            var newSession1 = this.sessionService.Create(sessionPlayer1_1, sessionPlayer2);
            var newSession2 = this.sessionService.Create(sessionPlayer1_3, sessionPlayer3);
            var newSession3 = this.sessionService.Create(sessionPlayer1_4, sessionPlayer4);

            this.sessionService.Start(newSession2);
            this.sessionService.Start(newSession3);

            var startedSession2 = this.sessionService.GetByName(newSession2.Name);
            var startedSession3 = this.sessionService.GetByName(newSession3.Name);

            this.sessionService.Finish(startedSession3.Name);

            var finishedSession3 = this.sessionService.GetByName(newSession3.Name);
            var finishedSessions = this.sessionService.GetFinished("player1");

            Assert.IsNotNull(finishedSessions);
            Assert.AreEqual(1, finishedSessions.Count());

            Assert.IsNotNull(newSession1);
            Assert.AreEqual("player1-vs-player2", newSession1.Name);
            Assert.AreEqual(SessionState.Pending, newSession1.State);
            Assert.IsTrue(newSession1.HasPlayer("player1"));
            Assert.IsTrue(newSession1.HasPlayer("player2"));
            Assert.AreEqual("player1", newSession1.GetVersusPlayer("player2").Information.Name);
            Assert.IsTrue(newSession1.Player1.IsReady);
            Assert.IsTrue(newSession1.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession1.Player1.SessionName);
            Assert.IsTrue(newSession1.Player2.IsReady);
            Assert.IsFalse(newSession1.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player2", newSession1.Player2.SessionName);

            Assert.IsNotNull(startedSession2);
            Assert.AreEqual("player1-vs-player3", startedSession2.Name);
            Assert.AreEqual(SessionState.Active, startedSession2.State);
            Assert.IsTrue(startedSession2.HasPlayer("player1"));
            Assert.IsTrue(startedSession2.HasPlayer("player3"));
            Assert.AreEqual("player1", startedSession2.GetVersusPlayer("player3").Information.Name);
            Assert.IsTrue(startedSession2.Player1.IsReady);
            Assert.IsTrue(startedSession2.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player3", startedSession2.Player1.SessionName);
            Assert.IsTrue(startedSession2.Player2.IsReady);
            Assert.IsFalse(startedSession2.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player3", startedSession2.Player2.SessionName);

            Assert.IsNotNull(finishedSession3);
            Assert.AreEqual("player1-vs-player4", finishedSession3.Name);
            Assert.AreEqual(SessionState.Finished, finishedSession3.State);
            Assert.IsTrue(finishedSession3.HasPlayer("player1"));
            Assert.IsTrue(finishedSession3.HasPlayer("player4"));
            Assert.AreEqual("player1", finishedSession3.GetVersusPlayer("player4").Information.Name);
            Assert.IsTrue(finishedSession3.Player1.IsReady);
            Assert.IsTrue(finishedSession3.Player1.PendingToMove);
            Assert.AreEqual("player1-vs-player4", finishedSession3.Player1.SessionName);
            Assert.IsTrue(finishedSession3.Player2.IsReady);
            Assert.IsFalse(finishedSession3.Player2.PendingToMove);
            Assert.AreEqual("player1-vs-player4", finishedSession3.Player2.SessionName);
        }
    }
}
