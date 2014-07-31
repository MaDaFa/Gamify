using Gamify.Client.Net.Contracts.Notifications;
using Gamify.Client.Net.Contracts.Requests;

namespace Gamify.Client.Net.Services
{
    public interface IGameService<TRequest, UNotification> : IGameSender<TRequest>, IGameListener<UNotification>
        where TRequest : IRequestObject
        where UNotification : INotificationObject
    {
    }
}
