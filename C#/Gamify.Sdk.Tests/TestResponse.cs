namespace Gamify.Sdk.Tests
{
    public class TestResponse : IGameMoveResponse<TestResultObject>
    {
        public bool IsWin { get; set; }

        object IGameMoveResponse.MoveResponseObject { get { return this.MoveResponseObject; } }

		public TestResultObject MoveResponseObject { get; private set; }

        public TestResponse(TestResultObject attemptResult)
		{
			this.MoveResponseObject = attemptResult;
		}
    }
}
