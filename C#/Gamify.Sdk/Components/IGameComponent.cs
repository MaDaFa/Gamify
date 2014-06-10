using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using System;

namespace Gamify.Sdk.Components
{
    public interface IGameComponentInformation
    {
        string Name { get; }
    }

    public interface IGameComponent : IGameComponentInformation
    {
        event EventHandler<GameNotificationEventArgs> Notification;

        bool CanHandleRequest(GameRequest request);

        bool CanHandleNotification(GameNotification notification);

        void HandleRequest(GameRequest request);
    }
}
