using System;

namespace GuessMyNumber.Client.Net
{
    public interface IGamifyService<TRequest, UNotification>
    {
        event EventHandler<GameNotificationEventArgs<UNotification>> NotificationReceived;

        void Send(TRequest request);
    }
}
