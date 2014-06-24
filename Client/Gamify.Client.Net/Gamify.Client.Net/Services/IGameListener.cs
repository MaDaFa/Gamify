using Gamify.Client.Net.Contracts.Notifications;
using System;

namespace Gamify.Client.Net.Services
{
    public interface IGameListener<TNotification> where TNotification : INotificationObject
    {
        event EventHandler<GameNotificationEventArgs<TNotification>> NotificationReceived;
    }
}
