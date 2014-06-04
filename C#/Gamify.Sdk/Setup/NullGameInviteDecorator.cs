using Gamify.Sdk.Contracts.Notifications;

namespace Gamify.Sdk.Setup
{
    public class NullGameInviteDecorator : IGameInviteDecorator
    {
        public void Decorate(GameInviteNotificationObject gameInviteNotification, IGameSession session)
        {
            return;
        }
    }
}
