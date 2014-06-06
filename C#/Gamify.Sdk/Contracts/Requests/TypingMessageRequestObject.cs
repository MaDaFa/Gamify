namespace Gamify.Sdk.Contracts.Requests
{
    public class TypingMessageRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }

        public string ToPlayerName { get; set; }

        public string TypingMessage { get; set; }
    }
}
