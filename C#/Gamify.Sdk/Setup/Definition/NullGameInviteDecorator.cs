using Gamify.Sdk.Contracts.Notifications;

namespace Gamify.Sdk.Setup.Definition
{
    public class NullGameInviteDecorator : IGameInviteDecorator
    {
        public void Decorate(GameInviteNotificationObject gameInviteNotification, IGameSession session)
        {
            return;
        }
    }
}
