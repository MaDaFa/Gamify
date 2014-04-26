namespace Gamify.Server.Contracts.Notifications
{
    public class SessionAbandonedNotificationObject : INotificationObject
    {
        public string Message
        {
            get
            {
                return string.Format("The session {0} has been abandoned by {1}", this.SessionId, this.PlayerName);
            }
        }

        public string SessionId { get; set; }

        public string PlayerName { get; set; }
    }
}
