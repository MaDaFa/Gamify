namespace Gamify.Server.Contracts.Notifications
{
    public class SessionOpenedNotificationObject : INotificationObject
    {
        public string Message
        {
            get
            {
                return string.Format("A new session between {0} and {1} has been opened", this.Player1Name, this.Player2Name);
            }
        }

        public string Player1Name { get; set; }

        public string Player2Name { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
