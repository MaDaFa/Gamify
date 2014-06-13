using Gamify.Sdk.Contracts.Notifications;

namespace Gamify.Sdk.Setup.Definition
{
    public interface IGameInformationNotificationFactory<TMove, UResponse>
    {
        GameInformationNotificationObject Create(IGameSession session, IPlayerHistoryItemFactory<TMove, UResponse> playerHistoryItemFactory);
    }
}
