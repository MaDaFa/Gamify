namespace Gamify.Contracts.Requests
{
    public class GetActiveGamesRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }
    }
}
