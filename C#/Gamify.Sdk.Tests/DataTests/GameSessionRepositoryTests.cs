using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gamify.Sdk.Tests.DataTests
{
    [TestClass]
    public class GameSessionRepositoryTests : RepositoryTests<GameSession>
    {
        [TestMethod]
        public void When_CreateGameSession_Then_Success()
        {
            var sessionName = "Session 1";
            var sessionHistoryService = Mock.Of<ISessionHistoryService<TestMoveObject, TestResponseObject>>();
            var player1 = new GamePlayer
            {
                Name = "Player 1",
                UserName = "player1"
            };
            var sessionPlayer1 = new TestSessionPlayer(sessionHistoryService)
            {
                SessionName = sessionName,
                PendingToMove = true,
                Information = player1
            };
            var player2 = new GamePlayer
            {
                Name = "Player 2",
                UserName = "player2"
            };
            var sessionPlayer2 = new TestSessionPlayer(sessionHistoryService)
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
            Assert.AreEqual(player1.UserName, createdSession.Player1.Information.UserName);
            Assert.AreEqual(player1.Name, createdSession.Player1.Information.Name);
            Assert.AreEqual(player2.UserName, createdSession.Player2.Information.UserName);
            Assert.AreEqual(player2.Name, createdSession.Player2.Information.Name);
        }
    }
}
