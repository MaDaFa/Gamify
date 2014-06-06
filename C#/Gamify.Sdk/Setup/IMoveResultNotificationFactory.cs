using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;

namespace Gamify.Sdk.Setup
{
    public interface IMoveResultNotificationFactory
    {
        IMoveResultNotificationObject Create(MoveRequestObject moveRequest, IGameMoveResponse moveResponse);
    }
}
