﻿namespace Gamify.Client.Net.Contracts.Notifications
{
    public class ErrorNotificationObject : INotificationObject
    {
        public int ErrorCode { get; set; }

        public string Message { get; set; }
    }
}
