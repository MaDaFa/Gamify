using Gamify.Service.Components;
using System;

namespace Gamify.Service
{
    public interface IGameServiceSetup
    {
        void RegisterComponent(IGameComponent component);
    }

    public interface IGameService : IGameServiceSetup
    {
        event EventHandler<GameNotificationEventArgs> Notification;

        void ConnectUser(string userName);

        void ReceiveMessage(string message);

        void DisconnectUser(string userName);
    }
}
