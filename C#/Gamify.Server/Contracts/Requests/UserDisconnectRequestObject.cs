namespace Gamify.Server.Contracts.Requests
{
    public class UserDisconnectRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }
    }
}
