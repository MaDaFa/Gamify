using Gamify.Sdk.Components;
using System;
using System.Collections.Generic;

namespace Gamify.Sdk.Setup
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

        Type GetGameBuilderType();
    }

    public interface IGameDefinition<TMove, UResponse> : IGameDefinition
    {
        new ISessionPlayerFactory<TMove, UResponse> GetSessionPlayerFactory();
    }
}
