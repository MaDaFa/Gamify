namespace Gamify.Sdk.Contracts.Notifications
{
    public class PlayerConnectedNotificationObject : INotificationObject
    {
        public string Message
        {
            get
            {
                return string.Format("{0} is now connected", this.PlayerName);
            }
        }

        public string PlayerName { get; set; }
    }
}
