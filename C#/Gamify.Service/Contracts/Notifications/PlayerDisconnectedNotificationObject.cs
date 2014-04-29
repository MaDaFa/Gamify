namespace Gamify.Service.Contracts.Notifications
{
    public class PlayerDisconnectedNotificationObject : INotificationObject
    {
        public string Message
        {
            get
            {
                return string.Format("Player {0} has been disconnected", this.PlayerName);
            }
        }

        public string PlayerName { get; set; }
    }
}
