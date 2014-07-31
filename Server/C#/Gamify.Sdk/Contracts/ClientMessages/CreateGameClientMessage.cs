using ThinkUp.Sdk.Contracts.ClientMessages;

namespace Gamify.Sdk.Contracts.ClientMessages
{
    public class CreateGameClientMessage : IClientMessage
    {
        public string UserName { get; set; }

        public string InvitedUserName { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
