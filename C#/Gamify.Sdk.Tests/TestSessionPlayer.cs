using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Tests
{
    public class TestSessionPlayer : SessionGamePlayer<TestMoveObject, TestResultObject>
    {
        public TestSessionPlayer(ISessionHistoryService<TestMoveObject, TestResultObject> sessionHistoryService)
            : base(sessionHistoryService)
        {
        }

        public override ISessionHistory<TestMoveObject, TestResultObject> GetHistory()
        {
            return this.sessionHistoryService.GetBySessionPlayer(this.SessionName, this.Information.UserName);
        }

        public override IGameMoveResponse<TestResultObject> ProcessMove(IGameMove<TestMoveObject> move)
        {
            var answer = move.MoveObject;
            var result = new TestResultObject { IsCorrect = false };
            var historyItem = new SessionHistoryItem<TestMoveObject, TestResultObject>(answer, result);

            this.sessionHistoryService.Add(this.SessionName, this.Information.UserName, historyItem);

            return new TestResponse(result);
        }
    }
}
