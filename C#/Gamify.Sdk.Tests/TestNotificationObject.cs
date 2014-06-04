using Gamify.Sdk.Contracts.Notifications;

namespace Gamify.Sdk.Tests
{
    public class TestNotificationObject : INotificationObject
    {
        public string Message
        {
            get
            {
                return string.Format("This is a message for {0}", this.Name); 
            }
        }

        public string Name { get; set; }
    }
}
