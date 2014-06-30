namespace Gamify.Sdk.Contracts.Requests
{
    public enum GameAuthenticationType : int
    {
        None = 0,
        Facebook = 1
    }

    public class PlayerConnectRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }

        public string AccessToken { get; set; }

        public int AuthenticationType { get; set; }
    }
}
