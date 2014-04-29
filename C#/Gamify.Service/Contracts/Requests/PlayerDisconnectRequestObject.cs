namespace Gamify.Service.Contracts.Requests
{
    public class PlayerDisconnectRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }
    }
}
