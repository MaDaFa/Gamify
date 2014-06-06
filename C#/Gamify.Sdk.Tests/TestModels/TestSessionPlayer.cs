using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Tests.TestModels
{
    public class TestSessionPlayer : SessionGamePlayer<TestMoveObject, TestResponseObject>
    {
        public TestSessionPlayer(ISessionHistoryService<TestMoveObject, TestResponseObject> sessionHistoryService)
            : base(sessionHistoryService)
        {
        }

        public override ISessionHistory<TestMoveObject, TestResponseObject> GetHistory()
        {
            return this.sessionHistoryService.GetBySessionPlayer(this.SessionName, this.Information.UserName);
        }

        public override IGameMoveResponse<TestResponseObject> ProcessMove(IGameMove<TestMoveObject> move)
        {
            var answer = move.MoveObject;
            var result = new TestResponseObject { IsCorrect = false };
            var historyItem = new SessionHistoryItem<TestMoveObject, TestResponseObject>(answer, result);

            this.sessionHistoryService.Add(this.SessionName, this.Information.UserName, historyItem);

            return new TestResponse(result);
        }
    }
}
