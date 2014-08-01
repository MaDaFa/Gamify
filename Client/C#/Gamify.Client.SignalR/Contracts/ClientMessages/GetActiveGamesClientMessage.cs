using ThinkUp.Sdk.Contracts.ClientMessages;

namespace Gamify.Sdk.Contracts.ClientMessages
{
    public class GetActiveGamesClientMessage : IClientMessage
    {
        public string UserName { get; set; }
    }
}
