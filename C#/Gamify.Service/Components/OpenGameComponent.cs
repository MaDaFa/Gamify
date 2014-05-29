using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Service.Interfaces;

namespace Gamify.Service.Components
{
    public abstract class OpenGameComponent : IGameComponent
    {
        protected readonly ISerializer<OpenGameRequestObject> serializer;
        protected readonly ISessionService sessionService;

        public INotificationService NotificationService { get; private set; }

        protected OpenGameComponent(ISessionService sessionService, INotificationService notificationService)
        {
            this.serializer = new JsonSerializer<OpenGameRequestObject>();
            this.sessionService = sessionService;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.OpenGame;
        }

        public void HandleRequest(GameRequest request)
        {
            var openGameObject = this.serializer.Deserialize(request.SerializedRequestObject);
            var currentSession = this.sessionService.GetByName(openGameObject.SessionName);

            this.SendGameInformation(openGameObject.PlayerName, currentSession);
        }

        protected abstract void SendGameInformation(string playerName, IGameSession gameSession);
    }
}
