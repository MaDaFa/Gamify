using Gamify.Sdk.Contracts.Notifications;
using System;

namespace Gamify.Sdk.Services
{
    public interface INotificationService
    {
        event EventHandler<GameNotificationEventArgs> Notification;

        void SendBroadcast(GameNotificationType gameNotificationType, object notificationObject, params string[] userNames);

        void SendBroadcast(int gameNotificationType, object notificationObject, params string[] userNames);

        void Send(GameNotificationType gameNotificationType, object notificationObject, string userName);

        void Send(int gameNotificationType, object notificationObject, string userName);

        void Send(GameNotification notification, string userName);
    }
}
