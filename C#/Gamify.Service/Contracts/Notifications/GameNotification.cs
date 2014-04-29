namespace Gamify.Service.Contracts.Notifications
{
    public enum GameNotificationType : int
    {
        PlayerConnected = 1,
        GameCreated = 2,
        GameInvite = 3,
        GameMove = 4,
        GameMoveResult = 5,
        GameAbandoned = 6,
        PlayerDisconnected = 7,
        Message = 254,
        Error = 255
    }

    public class GameNotification
    {
        public int Type { get; set; }

        public string SerializedNotificationObject { get; set; }
    }
}
