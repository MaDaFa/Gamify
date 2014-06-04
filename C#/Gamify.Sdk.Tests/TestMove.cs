namespace Gamify.Sdk.Tests
{
    public class TestMove : IGameMove<TestMoveObject>
    {
        public TestMoveObject MoveObject { get; private set; }

        public TestMove(TestMoveObject moveObject)
        {
            this.MoveObject = moveObject;
        }
    }
}
