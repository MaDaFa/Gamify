using Gamify.Contracts.Notifications;
using System;

namespace Gamify.Service.Interfaces
{
    public interface INotificationService
    {
        event EventHandler<GameNotificationEventArgs> Notification;

        void SendBroadcast(GameNotificationType gameNotificationType, object notificationObject, params string[] userNames);

        void Send(GameNotificationType gameNotificationType, object notificationObject, string userName);

        void Send(GameNotification notification, string userName);
    }
}
