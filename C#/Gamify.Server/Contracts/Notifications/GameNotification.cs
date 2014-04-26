namespace Gamify.Server.Contracts.Notifications
{
    public enum GameNotificationType : int
    {
        UserConnected = 1,
        SessionOpened = 2,
        GameMove = 3,
        SessionAbandoned = 4,
        UserDisconnected = 5,
        Message = 254,
        Error = 255
    }

    public class GameNotification
    {
        public GameNotificationType Type { get; set; }

        public string SerializedNotificationObject { get; set; }
    }
}
