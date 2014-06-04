using Gamify.Contracts.Requests;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Components
{
    public interface IGameComponent
    {
        INotificationService NotificationService { get; }

        bool CanHandleRequest(GameRequest request);

        void HandleRequest(GameRequest request);
    }
}
