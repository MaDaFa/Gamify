namespace Gamify.Service.Contracts.Requests
{
    public class GetActiveGamesRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }
    }
}
