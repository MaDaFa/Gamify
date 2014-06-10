namespace Gamify.Sdk.Contracts.Requests
{
    public class GameRejectedRequestObject : IRequestObject
    {
        public string SessionName { get; set; }

        public string PlayerName { get; set; }
    }
}
