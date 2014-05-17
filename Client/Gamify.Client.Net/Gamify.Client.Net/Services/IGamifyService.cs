using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using System;

namespace Gamify.Client.Net.Services
{
    public interface IGamifyService<TRequest, UNotification>
        where TRequest : IRequestObject
        where UNotification : INotificationObject
    {
        event EventHandler<GameNotificationEventArgs<UNotification>> NotificationReceived;

        void Send(TRequest request);
    }
}
