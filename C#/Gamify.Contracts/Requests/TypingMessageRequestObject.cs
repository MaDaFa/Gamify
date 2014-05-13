namespace Gamify.Contracts.Requests
{
    public class TypingMessageRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }

        public string ToPlayerName { get; set; }

        public string TypingMessage { get; set; }
    }
}
