using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gamify.Sdk.Tests.DataTests
{
    [TestClass]
    public class GameSessionRepositoryTests : RepositoryTests<GameSession>
    {
        [TestMethod]
        public void When_CreateGameSession_Then_Success()
        {
            var sessionName = GetUniqueName("Session");
            var player1 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = GetUniqueName("player")
            };
            var sessionPlayer1 = new TestSessionPlayer()
            {
                SessionName = sessionName,
                PendingToMove = true,
                Information = player1
            };
            var player2 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = GetUniqueName("player")
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                SessionName = sessionName,
                PendingToMove = false,
                Information = player2
            };
            var session = new GameSession(sessionPlayer1, sessionPlayer2)
            {
                Name = sessionName,
                State = SessionState.Active
            };

            this.testRepository.Create(session);

            var createdSession = this.testRepository.Get(s => s.Name == sessionName);

            Assert.IsNotNull(createdSession);
            Assert.AreEqual(session.Id, createdSession.Id);
            Assert.AreEqual(sessionName, createdSession.Name);
            Assert.AreEqual(SessionState.Active, createdSession.State);
            Assert.AreEqual(player1.Name, createdSession.Player1Name);
            Assert.AreEqual(player1.DisplayName, createdSession.Player1.Information.DisplayName);
            Assert.IsTrue(createdSession.Player1.PendingToMove);
            Assert.AreEqual(player2.Name, createdSession.Player2Name);
            Assert.AreEqual(player2.DisplayName, createdSession.Player2.Information.DisplayName);
            Assert.IsFalse(createdSession.Player2.PendingToMove);
        }

        [TestMethod]
        public void When_UpdateGameSession_Then_Success()
        {
            var sessionName = GetUniqueName("Session");
            var player1 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = GetUniqueName("player")
            };
            var sessionPlayer1 = new TestSessionPlayer()
            {
                SessionName = sessionName,
                PendingToMove = true,
                Information = player1
            };
            var player2 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = GetUniqueName("player")
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                SessionName = sessionName,
                PendingToMove = false,
                Information = player2
            };
            var session = new GameSession(sessionPlayer1, sessionPlayer2)
            {
                Name = sessionName,
                State = SessionState.Active
            };

            this.testRepository.Create(session);

            var createdSession = this.testRepository.Get(s => s.Name == sessionName);

            createdSession.State = SessionState.Abandoned;
            createdSession.Player1.PendingToMove = false;
            createdSession.Player2.PendingToMove = true;

            this.testRepository.Update(createdSession);

            var updatedSession = this.testRepository.Get(s => s.Name == sessionName);

            Assert.IsNotNull(updatedSession);
            Assert.AreEqual(session.Id, updatedSession.Id);
            Assert.AreEqual(sessionName, updatedSession.Name);
            Assert.AreEqual(SessionState.Abandoned, updatedSession.State);
            Assert.AreEqual(player1.Name, updatedSession.Player1Name);
            Assert.AreEqual(player1.DisplayName, updatedSession.Player1.Information.DisplayName);
            Assert.IsFalse(updatedSession.Player1.PendingToMove);
            Assert.AreEqual(player2.Name, updatedSession.Player2Name);
            Assert.AreEqual(player2.DisplayName, updatedSession.Player2.Information.DisplayName);
            Assert.IsTrue(updatedSession.Player2.PendingToMove);
        }

        [TestMethod]
        public void When_DeleteGameSession_Then_Success()
        {
            var sessionName = GetUniqueName("Session");
            var player1 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = GetUniqueName("player")
            };
            var sessionPlayer1 = new TestSessionPlayer()
            {
                SessionName = sessionName,
                PendingToMove = true,
                Information = player1
            };
            var player2 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = GetUniqueName("player")
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                SessionName = sessionName,
                PendingToMove = false,
                Information = player2
            };
            var session = new GameSession(sessionPlayer1, sessionPlayer2)
            {
                Name = sessionName,
                State = SessionState.Active
            };

            this.testRepository.Create(session);

            var createdSession = this.testRepository.Get(s => s.Name == sessionName);

            this.testRepository.Delete(createdSession);

            var deletedSession = this.testRepository.Get(s => s.Name == sessionName);

            Assert.IsNull(deletedSession);
        }

        [TestMethod]
        public void When_DeleteAllGameSessions_Then_Success()
        {
            var session1Name = GetUniqueName("Session");
            var player1 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = GetUniqueName("player")
            };
            var sessionPlayer1 = new TestSessionPlayer()
            {
                SessionName = session1Name,
                PendingToMove = true,
                Information = player1
            };
            var player2 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = GetUniqueName("player")
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                SessionName = session1Name,
                PendingToMove = false,
                Information = player2
            };
            var session1 = new GameSession(sessionPlayer1, sessionPlayer2)
            {
                Name = session1Name,
                State = SessionState.Active
            };

            var session2Name = GetUniqueName("Session");
            var player3 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = GetUniqueName("player")
            };
            var sessionPlayer3 = new TestSessionPlayer()
            {
                SessionName = session2Name,
                PendingToMove = true,
                Information = player2
            };
            var player4 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = GetUniqueName("player")
            };
            var sessionPlayer4 = new TestSessionPlayer()
            {
                SessionName = session2Name,
                PendingToMove = false,
                Information = player4
            };
            var session2 = new GameSession(sessionPlayer3, sessionPlayer4)
            {
                Name = session2Name,
                State = SessionState.Active
            };

            this.testRepository.Create(session1);
            this.testRepository.Create(session2);

            var currentSessionsCount = this.testRepository.GetAll().Count();

            this.testRepository.DeleteAll();

            var existingEntities = this.testRepository.GetAll();

            Assert.AreEqual(2, currentSessionsCount);
            Assert.AreEqual(0, existingEntities.Count());
        }

        [TestMethod]
        public void When_GetGameSessionsWithPredicate_Then_Success()
        {
            var session1Name = GetUniqueName("Session x");
            var player1 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = GetUniqueName("player")
            };
            var sessionPlayer1 = new TestSessionPlayer()
            {
                SessionName = session1Name,
                PendingToMove = true,
                Information = player1
            };
            var player2 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = GetUniqueName("player")
            };
            var sessionPlayer2 = new TestSessionPlayer()
            {
                SessionName = session1Name,
                PendingToMove = false,
                Information = player2
            };
            var session1 = new GameSession(sessionPlayer1, sessionPlayer2)
            {
                Name = session1Name,
                State = SessionState.Active
            };

            var session2Name = GetUniqueName("Session Number");
            var player3 = new GamePlayer
            {
                DisplayName = GetUniqueName("User"),
                Name = GetUniqueName("user")
            };
            var sessionPlayer3 = new TestSessionPlayer()
            {
                SessionName = session2Name,
                PendingToMove = true,
                Information = player3
            };
            var player4 = new GamePlayer
            {
                DisplayName = GetUniqueName("User"),
                Name = GetUniqueName("userx")
            };
            var sessionPlayer4 = new TestSessionPlayer()
            {
                SessionName = session2Name,
                PendingToMove = false,
                Information = player4
            };
            var session2 = new GameSession(sessionPlayer3, sessionPlayer4)
            {
                Name = session2Name,
                State = SessionState.Active
            };

            this.testRepository.Create(session1);
            this.testRepository.Create(session2);

            var userSession = this.testRepository.Get(s => s.Player1Name.Contains("user") ||
                s.Player2Name.Contains("user"));
            var filteredSessions = this.testRepository.GetAll(s => s.Name.Contains("x") ||
                s.Player1Name.Contains("x") || s.Player2Name.Contains("x"));

            Assert.IsNotNull(userSession);
            Assert.IsNotNull(filteredSessions);
            Assert.AreEqual(session2Name, userSession.Name);
            Assert.AreEqual(2, filteredSessions.Count());
        }
    }
}
