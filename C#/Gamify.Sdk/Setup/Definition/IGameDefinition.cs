using Gamify.Sdk.Components;
using System;
using System.Collections.Generic;

namespace Gamify.Sdk.Setup.Definition
{
    public interface IGameDefinition
    {
        ISessionPlayerFactory GetSessionPlayerFactory();

        ISessionPlayerSetup GetSessionPlayerSetup();

        IGameInviteDecorator GetGameInviteDecorator();

        IMoveHandler GetMoveHandler();

        IMoveResultNotificationFactory GetMoveResultNotificationFactory();

        IGameInformationNotificationFactory GetGameInformationNotificationFactory();

        IEnumerable<IGameComponent> GetCustomComponents();

        Type GetSessionHistoryServiceType();
    }

    public interface IGameDefinition<TMove, UResponse> : IGameDefinition
    {
        new ISessionPlayerFactory<TMove, UResponse> GetSessionPlayerFactory();
    }
}
