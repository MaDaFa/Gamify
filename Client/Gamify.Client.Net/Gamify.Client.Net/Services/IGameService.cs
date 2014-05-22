using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;

namespace Gamify.Client.Net.Services
{
    public interface IGameService<TRequest, UNotification> : IGameSender<TRequest>, IGameListener<UNotification>
        where TRequest : IRequestObject
        where UNotification : INotificationObject
    {
    }
}
