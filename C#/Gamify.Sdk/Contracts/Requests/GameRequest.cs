namespace Gamify.Sdk.Contracts.Requests
{
    public enum GameRequestType : int
    {
        PlayerConnect = 1,
        CreateGame = 2,
        GameAccepted = 3,
        GameRejected = 4,
        GameMove = 5,
        AbandonGame = 6,
        GetConnectedPlayers = 7,
        GetActiveGames = 8,
        GetPendingGames = 9,
        GetFinishedGames = 10,
        OpenGame = 11,
        Message = 12,
        TypingMessage = 13,
        PlayerDisconnect = 255
    }

    public class GameRequest
    {
        public int Type { get; set; }

        public string SerializedRequestObject { get; set; }

        public GameRequest(GameRequestType type)
        {
            this.Type = (int)type;
        }

        public GameRequest(int type)
        {
            this.Type = type;
        }

        public GameRequest()
        {
        }
    }
}
