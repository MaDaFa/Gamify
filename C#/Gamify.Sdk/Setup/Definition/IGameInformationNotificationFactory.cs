using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Setup.Definition
{
    public interface IGameInformationNotificationFactory<TMove, UResponse>
    {
        GameInformationNotificationObject Create(IGameSession session, ISessionHistoryService<TMove, UResponse> sessionHistoryService, IPlayerHistoryItemFactory<TMove, UResponse> playerHistoryItemFactory);
    }
}
