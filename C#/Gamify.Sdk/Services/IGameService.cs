using Gamify.Sdk.Components;
using System;
using System.Collections.Generic;

namespace Gamify.Sdk.Services
{
    public interface IGameServiceSetup
    {
        IEnumerable<IGameComponentInformation> Components { get; }

        void RegisterComponent(IGameComponent component);
    }

    public interface IGameService : IGameServiceSetup
    {
        event EventHandler<GameNotificationEventArgs> Notification;

        void Connect(string userName, string accessToken);

        void Send(string message);

        void Disconnect(string userName);
    }
}
