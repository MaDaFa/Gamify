using System;

namespace Gamify.Sdk.IntegrationTests.Setup
{
    public class TestResponse : IGameMoveResponse<TestResponseObject>
    {
        public TestResponseObject MoveResponseObject { get; set; }

        public bool IsWin { get; set; }

        object IGameMoveResponse.MoveResponseObject { get { return this.MoveResponseObject; } }

        public TestResponse(TestResponseObject moveResponseObject)
        {
            this.MoveResponseObject = moveResponseObject;
        }

        public TestResponse()
        {
        }
    }
}
