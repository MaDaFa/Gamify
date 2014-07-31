using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.IntegrationTests.Setup
{
    public class TestMoveFactory : IMoveFactory<TestMoveObject>
    {
        private readonly ISerializer serializer;

        public TestMoveFactory(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        public IGameMove<TestMoveObject> Create(string moveInformation)
        {
            var testMoveObject = this.serializer.Deserialize<TestMoveObject>(moveInformation);

            return new TestMove
            {
                MoveObject = testMoveObject
            };
        }
    }
}
