namespace Gamify.Server.Contracts.Requests
{
    public enum GameRequestType : int
    {
        UserConnect = 1,
        OpenSession = 2,
        GameAccepted = 3,
        GameMove = 4,
        AbandonSession = 5,
        UserDisconnect = 255
    }

    public class GameRequest
    {
        public GameRequestType Type { get; set; }

        public string SerializedRequestObject { get; set; }
    }
}
