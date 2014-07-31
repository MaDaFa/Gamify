using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gamify.Sdk.UnitTests.DataTests
{
    [TestClass]
    public class SessionHistoryRepositoryTests : RepositoryTests<SessionHistory<TestMoveObject, TestResponseObject>>
    {
        [TestMethod]
        public void UT_When_CreateSessionHistory_Then_Success()
        {
            var sessionName = GetUniqueName("Session");
            var playerName = GetUniqueName("player");
            var sessionHistory = new SessionHistory<TestMoveObject, TestResponseObject>(sessionName, playerName);

            sessionHistory.Add(new TestMoveObject { Answer = "Answer 1" }, new TestResponseObject { IsCorrect = false });
            sessionHistory.Add(new TestMoveObject { Answer = "Answer 2" }, new TestResponseObject { IsCorrect = false });
            sessionHistory.Add(new TestMoveObject { Answer = "Answer 3" }, new TestResponseObject { IsCorrect = true });
            sessionHistory.Add(new TestMoveObject { Answer = "Answer 4" }, new TestResponseObject { IsCorrect = false });
            sessionHistory.Add(new TestMoveObject { Answer = "Answer 5" }, new TestResponseObject { IsCorrect = true });

            this.testRepository.Create(sessionHistory);

            var createdSessionHistory = this.testRepository.Get(h => h.SessionName == sessionName && h.PlayerName == playerName);

            Assert.IsNotNull(createdSessionHistory);
            Assert.AreEqual(5, createdSessionHistory.Moves.Count);
        }

        [TestMethod]
        public void UT_When_UpdateSessionHistory_Then_Success()
        {
            var sessionName = GetUniqueName("Session");
            var playerName = GetUniqueName("player");
            var sessionHistory = new SessionHistory<TestMoveObject, TestResponseObject>(sessionName, playerName);

            sessionHistory.Add(new TestMoveObject { Answer = "Answer 1" }, new TestResponseObject { IsCorrect = false });
            sessionHistory.Add(new TestMoveObject { Answer = "Answer 2" }, new TestResponseObject { IsCorrect = false });
            sessionHistory.Add(new TestMoveObject { Answer = "Answer 3" }, new TestResponseObject { IsCorrect = true });
            sessionHistory.Add(new TestMoveObject { Answer = "Answer 4" }, new TestResponseObject { IsCorrect = false });
            sessionHistory.Add(new TestMoveObject { Answer = "Answer 5" }, new TestResponseObject { IsCorrect = true });

            this.testRepository.Create(sessionHistory);

            var createdSessionHistory = this.testRepository.Get(h => h.SessionName == sessionName && h.PlayerName == playerName);

            createdSessionHistory.Moves.RemoveAll(m => !m.Response.IsCorrect);

            this.testRepository.Update(createdSessionHistory);

            var updatedSessionHistory = this.testRepository.Get(h => h.SessionName == sessionName && h.PlayerName == playerName);

            Assert.IsNotNull(updatedSessionHistory);
            Assert.AreEqual(2, updatedSessionHistory.Moves.Count);
        }

        [TestMethod]
        public void UT_When_DeleteSessionHistory_Then_Success()
        {
            var sessionName = GetUniqueName("Session");
            var playerName = GetUniqueName("player");
            var sessionHistory = new SessionHistory<TestMoveObject, TestResponseObject>(sessionName, playerName);

            sessionHistory.Add(new TestMoveObject { Answer = "Answer 1" }, new TestResponseObject { IsCorrect = false });
            sessionHistory.Add(new TestMoveObject { Answer = "Answer 2" }, new TestResponseObject { IsCorrect = false });
            sessionHistory.Add(new TestMoveObject { Answer = "Answer 3" }, new TestResponseObject { IsCorrect = true });
            sessionHistory.Add(new TestMoveObject { Answer = "Answer 4" }, new TestResponseObject { IsCorrect = false });
            sessionHistory.Add(new TestMoveObject { Answer = "Answer 5" }, new TestResponseObject { IsCorrect = true });

            this.testRepository.Create(sessionHistory);

            var createdSessionHistory = this.testRepository.Get(h => h.SessionName == sessionName && h.PlayerName == playerName);

            this.testRepository.Delete(createdSessionHistory);

            var deletedSessionHistory = this.testRepository.Get(h => h.SessionName == sessionName && h.PlayerName == playerName);

            Assert.IsNull(deletedSessionHistory);
        }

        [TestMethod]
        public void UT_When_DeleteAllSessionHistories_Then_Success()
        {
            var session1Name = GetUniqueName("Session");
            var player1Name = GetUniqueName("player");
            var session1History = new SessionHistory<TestMoveObject, TestResponseObject>(session1Name, player1Name);

            session1History.Add(new TestMoveObject { Answer = "Answer 1" }, new TestResponseObject { IsCorrect = false });
            session1History.Add(new TestMoveObject { Answer = "Answer 2" }, new TestResponseObject { IsCorrect = false });
            session1History.Add(new TestMoveObject { Answer = "Answer 3" }, new TestResponseObject { IsCorrect = true });
            session1History.Add(new TestMoveObject { Answer = "Answer 4" }, new TestResponseObject { IsCorrect = false });
            session1History.Add(new TestMoveObject { Answer = "Answer 5" }, new TestResponseObject { IsCorrect = true });

            var session2Name = GetUniqueName("Session");
            var player2Name = GetUniqueName("player");
            var session2History = new SessionHistory<TestMoveObject, TestResponseObject>(session2Name, player2Name);

            session2History.Add(new TestMoveObject { Answer = "Best Answer 1" }, new TestResponseObject { IsCorrect = true });
            session2History.Add(new TestMoveObject { Answer = "Best Answer 2" }, new TestResponseObject { IsCorrect = true });

            var session3Name = GetUniqueName("Session");
            var player3Name = GetUniqueName("player");
            var session3History = new SessionHistory<TestMoveObject, TestResponseObject>(session3Name, player3Name);

            session3History.Add(new TestMoveObject { Answer = "Worst Answer 1" }, new TestResponseObject { IsCorrect = false });

            this.testRepository.Create(session1History);
            this.testRepository.Create(session2History);
            this.testRepository.Create(session3History);

            var existingSessionHistoriesCount = this.testRepository.GetAll().Count();

            this.testRepository.DeleteAll();

            var currentSessionHistories = this.testRepository.GetAll();

            Assert.AreEqual(3, existingSessionHistoriesCount);
            Assert.AreEqual(0, currentSessionHistories.Count());
        }

        [TestMethod]
        public void UT_When_GetSessionHistoriesWithPredicate_Then_Success()
        {
            var session1Name = GetUniqueName("Session");
            var player1Name = GetUniqueName("player");
            var session1History = new SessionHistory<TestMoveObject, TestResponseObject>(session1Name, player1Name);

            session1History.Add(new TestMoveObject { Answer = "Answer 1" }, new TestResponseObject { IsCorrect = false });
            session1History.Add(new TestMoveObject { Answer = "Answer 2" }, new TestResponseObject { IsCorrect = false });
            session1History.Add(new TestMoveObject { Answer = "Answer 3" }, new TestResponseObject { IsCorrect = true });
            session1History.Add(new TestMoveObject { Answer = "Answer 4" }, new TestResponseObject { IsCorrect = false });
            session1History.Add(new TestMoveObject { Answer = "Answer 5" }, new TestResponseObject { IsCorrect = true });

            var session2Name = GetUniqueName("Session");
            var player2Name = GetUniqueName("user");
            var session2History = new SessionHistory<TestMoveObject, TestResponseObject>(session2Name, player2Name);

            session2History.Add(new TestMoveObject { Answer = "Best Answer 1" }, new TestResponseObject { IsCorrect = true });
            session2History.Add(new TestMoveObject { Answer = "Best Answer 2" }, new TestResponseObject { IsCorrect = true });

            var session3Name = GetUniqueName("Worst Session");
            var player3Name = GetUniqueName("player");
            var session3History = new SessionHistory<TestMoveObject, TestResponseObject>(session3Name, player3Name);

            session3History.Add(new TestMoveObject { Answer = "Worst Answer 1" }, new TestResponseObject { IsCorrect = false });

            this.testRepository.Create(session1History);
            this.testRepository.Create(session2History);
            this.testRepository.Create(session3History);

            var worstHistories = this.testRepository.GetAll(h => h.SessionName.Contains("Worst"));
            var userHistories = this.testRepository.GetAll(h => h.PlayerName.Contains("user"));
            var longerHistories = this.testRepository.GetAll(h => h.Moves.Count > 3);
            var shorterHistories = this.testRepository.GetAll(h => h.Moves.Count <= 2);
            var allWrongHistory = this.testRepository.Get(h => !h.Moves.Any(m => m.Response.IsCorrect));

            Assert.AreEqual(1, worstHistories.Count());
            Assert.AreEqual(1, userHistories.Count());
            Assert.AreEqual(1, longerHistories.Count());
            Assert.AreEqual(2, shorterHistories.Count());
            Assert.IsNotNull(allWrongHistory);
            Assert.AreEqual(session3Name, allWrongHistory.SessionName);
            Assert.AreEqual(player3Name, allWrongHistory.PlayerName);
        }
    }
}
