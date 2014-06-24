namespace Gamify.Client.Net.Contracts.Notifications
{
    public class GameAbandonedNotificationObject : INotificationObject
    {
        public string Message
        {
            get
            {
                return string.Format("The game {0} has been abandoned by {1}", this.SessionName, this.PlayerName);
            }
        }

        public string SessionName { get; set; }

        public string PlayerName { get; set; }
    }
}
