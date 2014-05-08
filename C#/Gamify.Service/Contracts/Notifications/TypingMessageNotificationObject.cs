namespace Gamify.Service.Contracts.Notifications
{
    public class TypingMessageNotificationObject : INotificationObject
    {
        public string FromPlayerName { get; set; }

        public string Message { get; set; }
    }
}
