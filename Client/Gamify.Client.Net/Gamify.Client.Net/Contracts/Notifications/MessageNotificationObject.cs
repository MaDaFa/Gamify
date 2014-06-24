namespace Gamify.Client.Net.Contracts.Notifications
{
    public class MessageNotificationObject : INotificationObject
    {
        public string FromPlayerName { get; set; }

        public string Message { get; set; }
    }
}
