using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Components
{
    public abstract class GameComponent : IGameComponent
    {
        public string Name
        {
            get { return this.GetType().Name; }
        }

        public INotificationService NotificationService { get; private set; }

        public GameComponent(INotificationService notificationService)
        {
            this.NotificationService = notificationService;
        }

        public abstract bool CanHandleRequest(GameRequest request);

        public abstract void HandleRequest(GameRequest request);
    }
}
