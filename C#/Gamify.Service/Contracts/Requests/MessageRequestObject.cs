namespace Gamify.Service.Contracts.Requests
{
    public class MessageRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }

        public string ToPlayerName { get; set; } 

        public string Message { get; set; }
    }
}
