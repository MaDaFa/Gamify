using Gamify.Sdk.Contracts.Notifications;
using System;

namespace Gamify.Sdk
{
    public class GameNotificationEventArgs : EventArgs
    {
        public string Receiver { get; set; }

        public GameNotification Notification { get; set; }

        public GameNotificationEventArgs(string receiver, GameNotification notification)
        {
            this.Receiver = receiver;
            this.Notification = notification;
        }

        public GameNotificationEventArgs()
        {
        }
    }
}
