using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;

namespace Gamify.Sdk.Setup.Definition
{
    public interface IMoveResultNotificationFactory
    {
        IMoveResultNotificationObject Create(MoveRequestObject moveRequest, IGameMoveResponse moveResponse);
    }
}
