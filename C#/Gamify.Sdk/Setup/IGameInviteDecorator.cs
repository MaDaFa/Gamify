using Gamify.Sdk.Contracts.Notifications;

namespace Gamify.Sdk.Setup
{
    public interface IGameInviteDecorator
    {
        void Decorate(GameInviteNotificationObject gameInviteNotification, IGameSession session);
    }
}
