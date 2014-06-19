using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gamify.Sdk.UnitTests.ServiceTests
{
    [TestClass]
    public class SessionHistoryServiceTests
    {
        private ISessionHistoryService<TestMoveObject, TestResponseObject> sessionHistoryService;

        [TestInitialize]
        public void Initialize()
        {
            var sessionHistoryRepository = new TestRepository<SessionHistory<TestMoveObject, TestResponseObject>>();

            this.sessionHistoryService = new SessionHistoryService<TestMoveObject, TestResponseObject>(sessionHistoryRepository);
        }

        [TestMethod]
        public void When_CreateHistory_Then_Success()
        {
            var sessionName = "Session 1";
            var playerName = "player1";
            var sessionHistoryItem = new SessionHistoryItem<TestMoveObject, TestResponseObject>()
            {
                Move = new TestMoveObject { Answer = "Test Answer 1" },
                Response = new TestResponseObject { IsCorrect = false }
            };

            this.sessionHistoryService.Add(sessionName, playerName, sessionHistoryItem);

            var history = this.sessionHistoryService.GetBySessionPlayer(sessionName, playerName);

            Assert.IsNotNull(history);
            Assert.AreEqual(1, history.Get().Count());
        }

        [TestMethod]
        public void When_UpdateHistory_Then_Success()
        {
            var sessionName = "Session 1";
            var playerName = "player1";
            var sessionHistoryItem1 = new SessionHistoryItem<TestMoveObject, TestResponseObject>()
            {
                Move = new TestMoveObject { Answer = "Test Answer 1" },
                Response = new TestResponseObject { IsCorrect = false }
            };

            this.sessionHistoryService.Add(sessionName, playerName, sessionHistoryItem1);

            var sessionHistoryItem2 = new SessionHistoryItem<TestMoveObject, TestResponseObject>()
            {
                Move = new TestMoveObject { Answer = "Test Answer 2" },
                Response = new TestResponseObject { IsCorrect = true }
            };
            var sessionHistoryItem3 = new SessionHistoryItem<TestMoveObject, TestResponseObject>()
            {
                Move = new TestMoveObject { Answer = "Test Answer 3" },
                Response = new TestResponseObject { IsCorrect = true }
            };

            this.sessionHistoryService.Add(sessionName, playerName, sessionHistoryItem2);
            this.sessionHistoryService.Add(sessionName, playerName, sessionHistoryItem3);

            var history = this.sessionHistoryService.GetBySessionPlayer(sessionName, playerName);

            Assert.IsNotNull(history);
            Assert.AreEqual(3, history.Get().Count());
        }

        [TestMethod]
        public void When_CheckExistingHistory_Then_Success()
        {
            var sessionName = "Session 1";
            var playerName = "player1";
            var existPreviousHistory = this.sessionHistoryService.Exist(sessionName, playerName);
            var sessionHistoryItem1 = new SessionHistoryItem<TestMoveObject, TestResponseObject>()
            {
                Move = new TestMoveObject { Answer = "Test Answer 1" },
                Response = new TestResponseObject { IsCorrect = false }
            };

            this.sessionHistoryService.Add(sessionName, playerName, sessionHistoryItem1);

            var existCurrentHistory = this.sessionHistoryService.Exist(sessionName, playerName);

            Assert.IsFalse(existPreviousHistory);
            Assert.IsTrue(existCurrentHistory);
        }
    }
}
