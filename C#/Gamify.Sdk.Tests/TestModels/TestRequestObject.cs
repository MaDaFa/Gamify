using Gamify.Contracts.Requests;

namespace Gamify.Sdk.Tests.TestModels
{
    public class TestRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }

        public string TestValue { get; set; }
    }
}
