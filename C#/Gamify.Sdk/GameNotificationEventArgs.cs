using Gamify.Sdk.Contracts.Notifications;
using System;

namespace Gamify.Sdk
{
    public class GameNotificationEventArgs : EventArgs
    {
        public string UserName { get; set; }

        public GameNotification Notification { get; set; }

        public GameNotificationEventArgs(string userName, GameNotification notification)
        {
            this.UserName = userName;
            this.Notification = notification;
        }

        public GameNotificationEventArgs()
        {
        }
    }
}
