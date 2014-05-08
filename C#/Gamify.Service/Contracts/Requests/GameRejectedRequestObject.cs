namespace Gamify.Service.Contracts.Requests
{
    public class GameRejectedRequestObject : IRequestObject
    {
        public string SessionId { get; set; }

        public string PlayerName { get; set; }
    }
}
