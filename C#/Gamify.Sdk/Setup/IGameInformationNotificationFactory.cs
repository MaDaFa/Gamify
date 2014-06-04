using Gamify.Sdk.Contracts.Notifications;

namespace Gamify.Sdk.Setup
{
    public interface IGameInformationNotificationFactory
    {
        GameInformationNotificationObject Create(IGameSession session);
    }
}
