using Gamify.Sdk.Contracts.Requests;

namespace Gamify.Sdk.UnitTests.TestModels
{
    public class TestRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }

        public string TestValue { get; set; }
    }
}
