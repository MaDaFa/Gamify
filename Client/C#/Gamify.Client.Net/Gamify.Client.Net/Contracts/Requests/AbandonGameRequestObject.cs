namespace Gamify.Client.Net.Contracts.Requests
{
    public class AbandonGameRequestObject : IRequestObject
    {
        public string SessionName { get; set; }

        public string PlayerName { get; set; }
    }
}
