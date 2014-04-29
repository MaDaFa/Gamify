namespace Gamify.Service.Contracts.Requests
{
    public class CreateGameRequestObject : IRequestObject
    {
        public string PlayerName { get; set; }

        public string InvitedPlayerName { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
