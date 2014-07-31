using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Interfaces;

namespace Gamify.Sdk.Setup.Definition
{
    public class NullGameInviteDecorator : IGameInviteDecorator
    {
        public void Decorate(GameInviteReceivedServerMessage gameInviteNotification, IGameSession session)
        {
            return;
        }
    }
}
