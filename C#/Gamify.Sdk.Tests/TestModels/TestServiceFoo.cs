namespace Gamify.Sdk.UnitTests.TestModels
{
    public class TestServiceFoo : ITestServiceFoo
    {
        public ITestServiceBar TestServiceBar { get; private set; }

        public TestServiceFoo(ITestServiceBar testServiceBar)
        {
            this.TestServiceBar = testServiceBar;
        }

        public void TestFoo()
        {
            return;
        }
    }
}
