using ThinkUp.Sdk.Contracts.ClientMessages;

namespace Gamify.Sdk.Contracts.ClientMessages
{
    public class GetPendingGamesClientMessage : IClientMessage
    {
        public string UserName { get; set; }
    }
}
