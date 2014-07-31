namespace Gamify.Sdk.UnitTests.TestModels
{
    public class TestSessionPlayer : SessionGamePlayer
    {
        //public override ISessionHistory<TestMoveObject, TestResponseObject> GetHistory()
        //{
        //    return this.sessionHistoryService.GetBySessionPlayer(this.SessionName, this.Information.Name);
        //}

        //public override IGameMoveResponse<TestResponseObject> ProcessMove(IGameMove<TestMoveObject> move)
        //{
        //    var answer = move.MoveObject;
        //    var result = new TestResponseObject { IsCorrect = false };
        //    var historyItem = new SessionHistoryItem<TestMoveObject, TestResponseObject>(answer, result);

        //    this.sessionHistoryService.Add(this.SessionName, this.Information.Name, historyItem);

        //    return new TestResponse(result);
        //}

        string Keyword { get; set; }
    }
}
