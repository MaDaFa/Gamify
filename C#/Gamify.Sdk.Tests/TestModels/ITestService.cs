namespace Gamify.Sdk.UnitTests.TestModels
{
    public interface ITestServiceFoo
    {
        ITestServiceBar TestServiceBar { get; }

        void TestFoo();
    }
}
