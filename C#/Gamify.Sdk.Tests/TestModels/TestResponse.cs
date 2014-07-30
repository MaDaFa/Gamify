using Gamify.Sdk.Interfaces;

namespace Gamify.Sdk.UnitTests.TestModels
{
    public class TestResponse : IGameMoveResponse<TestResponseObject>
    {
        public bool IsWin { get; set; }

        object IGameMoveResponse.MoveResponseObject { get { return this.MoveResponseObject; } }

		public TestResponseObject MoveResponseObject { get; private set; }

        public TestResponse(TestResponseObject attemptResult)
		{
			this.MoveResponseObject = attemptResult;
		}
    }
}
