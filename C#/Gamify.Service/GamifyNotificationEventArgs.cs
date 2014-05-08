using System;

namespace Gamify.Service
{
    public class GamifyNotificationEventArgs : EventArgs
    {
        public string UserName { get; set; }

        public string  Notification { get; set; }

        public GamifyNotificationEventArgs(string userName, string notification)
        {
            this.UserName = userName;
            this.Notification = notification;
        }

        public GamifyNotificationEventArgs()
        {
        }
    }
}
