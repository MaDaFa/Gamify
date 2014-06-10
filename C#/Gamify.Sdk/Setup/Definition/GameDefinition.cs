﻿using Gamify.Sdk.Components;
using Gamify.Sdk.Services;
using System;
using System.Collections.Generic;

namespace Gamify.Sdk.Setup.Definition
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

        public Type GetSessionHistoryServiceType()
        {
            return typeof(SessionHistoryService<TMove, UResponse>);
        }
    }
}