using Gamify.Sdk.Components;
using System.Collections.Generic;

namespace Gamify.Sdk.Setup.Definition
{
    public interface IGameDefinition<TMove, UResponse>
    {
        ISessionPlayerFactory GetSessionPlayerFactory();

        ISessionPlayerSetup GetSessionPlayerSetup();

        IMoveFactory<TMove> GetMoveFactory();

        IMoveProcessor<TMove, UResponse> GetMoveProcessor();

        IMoveResultNotificationFactory GetMoveResultNotificationFactory();

        IGameInviteDecorator GetGameInviteDecorator();

        IGameInformationNotificationFactory<TMove, UResponse> GetGameInformationNotificationFactory();

        IPlayerHistoryItemFactory<TMove, UResponse> GetPlayerHistoryItemfactory();

        IEnumerable<IGameComponent> GetCustomComponents();
    }
}
