namespace Gamify.Server.Contracts.Notifications
{
    public enum GameNotificationType : int
    {
        UserConnected = 1,
        SessionOpened = 2,
        GameInvite = 3,
        GameMove = 4,
        SessionAbandoned = 5,
        UserDisconnected = 6,
        Message = 254,
        Error = 255
    }

    public class GameNotification
    {
        public GameNotificationType Type { get; set; }

        public string SerializedNotificationObject { get; set; }
    }
}
