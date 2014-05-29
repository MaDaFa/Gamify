using Gamify.Contracts.Requests;
using Gamify.Service.Interfaces;

namespace Gamify.Service.Components
{
    public interface IGameComponent
    {
        INotificationService NotificationService { get; }

        bool CanHandleRequest(GameRequest request);

        void HandleRequest(GameRequest request);
    }
}
