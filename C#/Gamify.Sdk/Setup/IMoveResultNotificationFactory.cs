using Gamify.Contracts.Requests;
using Gamify.Sdk.Contracts.Notifications;

namespace Gamify.Sdk.Setup
{
    public interface IMoveResultNotificationFactory
    {
        IMoveResultNotificationObject Create(MoveRequestObject moveRequest, IGameMoveResponse moveResponse);
    }
}
