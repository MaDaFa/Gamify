using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.IntegrationTests.Setup
{
    public class TestGameDefinition : GameDefinition<TestMoveObject, TestResponseObject>
    {
        private readonly ISerializer serializer;

        public TestGameDefinition(ISerializer serializer)
        {
            this.serializer = serializer;
        }

        public override IMoveFactory<TestMoveObject> GetMoveFactory()
        {
            return new TestMoveFactory(this.serializer);
        }

        public override IMoveProcessor<TestMoveObject, TestResponseObject> GetMoveProcessor()
        {
            return new TestMoveProcessor();
        }

        public override IMoveResultNotificationFactory GetMoveResultNotificationFactory()
        {
            return new TestMoveResultNotificationFactory();
        }

        public override IPlayerHistoryItemFactory<TestMoveObject, TestResponseObject> GetPlayerHistoryItemfactory()
        {
            return new TestPlayerHistoryItemFactory();
        }
    }
}
