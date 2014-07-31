namespace Gamify.Client.Net.Contracts.Requests
{
    public class MoveRequestObject : IRequestObject
    {
        public string SessionName { get; set; }

        public string PlayerName { get; set; }

        public string MoveInformation { get; set; }
    }
}
