using Gamify.Sdk.Contracts.Notifications;

namespace Gamify.Sdk.Setup.Definition
{
    public interface IGameInviteDecorator
    {
        void Decorate(GameInviteNotificationObject gameInviteNotification, IGameSession session);
    }
}
