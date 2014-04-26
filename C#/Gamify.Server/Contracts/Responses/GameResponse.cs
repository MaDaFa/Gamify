namespace Gamify.Server.Contracts.Responses
{
    public enum GameResponseType : int
    {
        GameMove = 1,
        Error = 255
    }

    public class GameResponse
    {
        public GameResponseType Type { get; set; }

        public string SerializedResponseObject { get; set; }
    }
}
