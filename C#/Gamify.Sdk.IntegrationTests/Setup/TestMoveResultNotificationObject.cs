using Gamify.Sdk.Contracts.Notifications;

namespace Gamify.Sdk.IntegrationTests.Setup
{
    public class TestMoveResultNotificationObject : IMoveResultNotificationObject
    {
        public string SessionName { get; set; }

        public string PlayerName { get; set; }

        public string Message { get; set; }

        public bool AnsweredCorrect { get; set; }
    }
}
