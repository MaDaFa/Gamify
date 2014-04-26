namespace Gamify.Server.Contracts.Requests
{
    public enum GameRequestType : int
    {
        UserConnect = 1,
        OpenSession = 2,
        GameMove = 3,
        AbandonSession = 4,
        UserDisconnect = 255
    }

    public class GameRequest
    {
        public GameRequestType Type { get; set; }

        public string SerializedRequestObject { get; set; }
    }
}
