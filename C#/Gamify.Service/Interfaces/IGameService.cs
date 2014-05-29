using Gamify.Service.Components;
using System;

namespace Gamify.Service.Interfaces
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
