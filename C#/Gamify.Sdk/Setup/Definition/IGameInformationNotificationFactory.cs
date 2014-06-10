using Gamify.Sdk.Contracts.Notifications;

namespace Gamify.Sdk.Setup.Definition
{
    public interface IGameInformationNotificationFactory
    {
        GameInformationNotificationObject Create(IGameSession session);
    }
}
