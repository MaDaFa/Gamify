namespace Gamify.Sdk.Contracts.Notifications
{
    public enum GameNotificationType : int
    {
        PlayerConnected = 1,
        GameInvite = 2,
        GameCreated = 3,
        GameRejected = 4,
        GameMove = 5,
        GameMoveResult = 6,
        GameAbandoned = 7,
        SendConnectedPlayers = 8,
        SendActiveGames = 9,
        SendGameInformation = 10,
        GameFinished = 11,
        Message = 12,
        TypingMessage = 13,
        PlayerDisconnected = 14,
        Error = 255
    }

    public class GameNotification
    {
        public int Type { get; set; }

        public string SerializedNotificationObject { get; set; }

        public GameNotification(GameNotificationType type)
        {
            this.Type = (int)type;
        }

        public GameNotification(int type)
        {
            this.Type = type;
        }

        public GameNotification()
        {
        }
    }
}
