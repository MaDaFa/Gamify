namespace Gamify.Server.Contracts.Requests
{
    public class PlayerDisconnectRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }
    }
}
