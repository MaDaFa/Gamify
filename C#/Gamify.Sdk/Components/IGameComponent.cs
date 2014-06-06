using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Components
{
    public interface IGameComponentInformation
    {
        string Name { get; }
    }

    public interface IGameComponent : IGameComponentInformation
    {
        INotificationService NotificationService { get; }

        bool CanHandleRequest(GameRequest request);

        void HandleRequest(GameRequest request);
    }
}
