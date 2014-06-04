using Gamify.Sdk.Components;
using System;

namespace Gamify.Sdk.Services
{
    public interface IGameServiceSetup
    {
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
