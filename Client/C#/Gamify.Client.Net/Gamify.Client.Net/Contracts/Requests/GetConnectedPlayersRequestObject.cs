namespace Gamify.Client.Net.Contracts.Requests
{
    public class GetConnectedPlayersRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }

        public int PageSize { get; set; }
    }
}
