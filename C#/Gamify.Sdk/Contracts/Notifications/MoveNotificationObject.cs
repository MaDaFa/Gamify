namespace Gamify.Sdk.Contracts.Notifications
{
    public class MoveNotificationObject : INotificationObject
    {
        public string Message
        {
            get
            {
                return string.Format("The player {0} has tried the {1} move", this.PlayerName, this.MoveInformation);
            }
        }

        public string SessionName { get; set; }

        public string PlayerName { get; set; }

        public string MoveInformation { get; set; }
    }
}
