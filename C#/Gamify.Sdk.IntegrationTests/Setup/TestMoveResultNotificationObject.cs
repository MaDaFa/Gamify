using Gamify.Sdk.Contracts.ServerMessages;

namespace Gamify.Sdk.IntegrationTests.Setup
{
    public class TestMoveResultNotificationObject : IMoveResultReceivedServerMessage
    {
        public string SessionName { get; set; }

        public string PlayerName { get; set; }

        public string Message { get; set; }

        public bool AnsweredCorrect { get; set; }
    }
}
