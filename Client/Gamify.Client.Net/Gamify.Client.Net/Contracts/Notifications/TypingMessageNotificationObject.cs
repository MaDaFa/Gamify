namespace Gamify.Client.Net.Contracts.Notifications
{
    public class TypingMessageNotificationObject : INotificationObject
    {
        public string FromPlayerName { get; set; }

        public string Message { get; set; }
    }
}
