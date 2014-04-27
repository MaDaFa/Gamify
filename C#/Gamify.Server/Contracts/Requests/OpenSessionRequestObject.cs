namespace Gamify.Server.Contracts.Requests
{
    public class OpenSessionRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }

        public string VersusPlayerName { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
