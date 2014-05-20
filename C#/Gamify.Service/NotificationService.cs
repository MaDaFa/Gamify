using Gamify.Contracts.Notifications;
using Gamify.Core;
using Gamify.Core.Interfaces;
using System;

namespace Gamify.Service
{
    public class NotificationService : INotificationService
    {
        private readonly ISerializer<object> serializer;

        public event EventHandler<GameNotificationEventArgs> Notification;

        public NotificationService()
        {
            this.serializer = new JsonSerializer<object>();
        }

        public void SendBroadcast(GameNotificationType gameNotificationType, object notificationObject, params string[] userNames)
        {
            foreach (var userName in userNames)
            {
                this.Send(gameNotificationType, notificationObject, userName);
            }
        }

        public void Send(GameNotificationType gameNotificationType, object notificationObject, string userName)
        {
            var notification = new GameNotification
            {
                Type = (int)gameNotificationType,
                SerializedNotificationObject = this.serializer.Serialize(notificationObject)
            };

            this.Send(notification, userName);
        }

        public void Send(GameNotification notification, string userName)
        {
            var sendMessageHandler = this.Notification;

            if (sendMessageHandler != null)
            {
                sendMessageHandler(this, new GameNotificationEventArgs(userName, notification));
            }
        }
    }
}
