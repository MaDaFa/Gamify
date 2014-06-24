using Gamify.Sdk.Contracts.Notifications;

namespace Gamify.Sdk.IntegrationTests.Setup
{
    public class TestPlayerHistoryItem : IPlayerHistoryItem
    {
        public string Question { get; set; }

        public string Answer { get; set; }

        public bool Correct { get; set; }
    }
}
