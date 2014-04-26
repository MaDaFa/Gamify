namespace Gamify.Server.Contracts.Requests
{
    public class AbandonSessionRequestObject : IRequestObject
    {
        public string SessionId { get; set; }

        public string PlayerName { get; set; }
    }
}
