namespace Gamify.Server.Contracts.Notifications
{
    public class GameAbandonedNotificationObject : INotificationObject
    {
        public string Message
        {
            get
            {
                return string.Format("The game {0} has been abandoned by {1}", this.SessionId, this.PlayerName);
            }
        }

        public string SessionId { get; set; }

        public string PlayerName { get; set; }
    }
}
