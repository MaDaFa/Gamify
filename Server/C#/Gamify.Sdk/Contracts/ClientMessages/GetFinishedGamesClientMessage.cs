using ThinkUp.Sdk.Contracts.ClientMessages;

namespace Gamify.Sdk.Contracts.ClientMessages
{
    public class GetFinishedGamesClientMessage : IClientMessage
    {
        public string UserName { get; set; }
    }
}
