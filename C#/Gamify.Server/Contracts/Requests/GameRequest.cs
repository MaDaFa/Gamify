namespace Gamify.Server.Contracts.Requests
{
    public enum GameRequestType : int
    {
        PlayerConnect = 1,
        CreateGame = 2,
        GameAccepted = 3,
        GameMove = 4,
        AbandonGame = 5,
        PlayerDisconnect = 255
    }

    public class GameRequest
    {
        public int Type { get; set; }

        public string SerializedRequestObject { get; set; }
    }
}
