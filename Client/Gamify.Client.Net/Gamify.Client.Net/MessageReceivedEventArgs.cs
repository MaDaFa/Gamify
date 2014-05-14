using Gamify.Contracts.Notifications;
using System;

namespace GuessMyNumber.Client.Net
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public GameNotification NotificationObject { get; private set; }

        public MessageReceivedEventArgs(GameNotification notificationObject)
        {
            this.NotificationObject = notificationObject;
        }
    }
}
