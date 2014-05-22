using Gamify.Contracts.Notifications;
using System;

namespace Gamify.Client.Net.Client
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public GameNotification GameNotification { get; private set; }

        public MessageReceivedEventArgs(GameNotification notificationObject)
        {
            this.GameNotification = notificationObject;
        }
    }
}
