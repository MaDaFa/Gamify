using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Interfaces;

namespace Gamify.Sdk.Setup.Definition
{
    public interface IMoveResultNotificationFactory
    {
        IMoveResultReceivedServerMessage Create(SendMoveClientMessage moveRequest, IGameMoveResponse moveResponse);
    }
}
