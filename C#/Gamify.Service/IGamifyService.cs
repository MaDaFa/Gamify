using System;

namespace Gamify.Service
{
    public interface IGamifyService
    {
        event EventHandler<GamifyNotificationEventArgs> Notification;

        void ConnectUser(string userName);

        void OnReceive(string userName, string message);

        void OnDisconnect(string userName);

        void OnError(string userName, Exception ex);
    }
}
