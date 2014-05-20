using Gamify.Service.Components;
using System;

namespace Gamify.Service
{
    public interface IGameService
    {
        event EventHandler<GameNotificationEventArgs> Notification;

        void RegisterComponent(IGameComponent component);

        void ConnectUser(string userName);

        void ReceiveMessage(string message);

        void DisconnectUser(string userName);
    }
}
