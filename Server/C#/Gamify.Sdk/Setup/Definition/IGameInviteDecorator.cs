using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Interfaces;

namespace Gamify.Sdk.Setup.Definition
{
    public interface IGameInviteDecorator
    {
        void Decorate(GameInviteReceivedServerMessage gameInviteNotification, IGameSession session);
    }
}
