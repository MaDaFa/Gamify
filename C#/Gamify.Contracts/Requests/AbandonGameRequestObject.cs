namespace Gamify.Contracts.Requests
{
    public class AbandonGameRequestObject : IRequestObject
    {
        public string SessionId { get; set; }

        public string PlayerName { get; set; }
    }
}
