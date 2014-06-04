using Gamify.Sdk.Components;
using System;
using System.Collections.Generic;

namespace Gamify.Sdk.Setup
{
    public abstract class GameDefinition<TMove, UResponse> : IGameDefinition<TMove, UResponse>
    {
        ISessionPlayerFactory IGameDefinition.GetSessionPlayerFactory()
        {
            return this.GetSessionPlayerFactory();
        }

        public abstract ISessionPlayerFactory<TMove, UResponse> GetSessionPlayerFactory();

        public virtual ISessionPlayerSetup GetSessionPlayerSetup()
        {
            return new NullSessionPlayerSetup();
        }

        public virtual IGameInviteDecorator GetGameInviteDecorator()
        {
            return new NullGameInviteDecorator();
        }

        public abstract IMoveHandler GetMoveHandler();

        public abstract IMoveResultNotificationFactory GetMoveResultNotificationFactory();

        public abstract IGameInformationNotificationFactory GetGameInformationNotificationFactory();

        public virtual IEnumerable<IGameComponent> GetCustomComponents()
        {
            return new List<IGameComponent>();
        }

        public Type GetGameBuilderType()
        {
            return typeof(GameBuilder<TMove, UResponse>);
        }
    }
}
