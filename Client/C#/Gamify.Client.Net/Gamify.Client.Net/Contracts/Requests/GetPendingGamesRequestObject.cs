namespace Gamify.Client.Net.Contracts.Requests
{
    public class GetPendingGamesRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }
    }
}
