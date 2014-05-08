namespace Gamify.Service.Contracts.Notifications
{
    public class GameRejectedNotificationObject : INotificationObject
    {
        public string Message
        {
            get
            {
                return string.Format("Player {0} has rejected the invitation to join the game", this.Player2Name);
            }
        }

        public string SessionId { get; set; }

        public string Player1Name { get; set; }

        public string Player2Name { get; set; }
    }
}
