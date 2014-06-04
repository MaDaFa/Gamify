using Gamify.Contracts.Requests;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;

namespace Gamify.Sdk.Components
{
    public class OpenGameComponent : IGameComponent
    {
        private readonly ISessionService sessionService;
        private readonly IGameInformationNotificationFactory gameInformationNotificationFactory;
        private readonly ISerializer serializer;

        public INotificationService NotificationService { get; private set; }

        public OpenGameComponent(ISessionService sessionService, INotificationService notificationService,
            IGameInformationNotificationFactory gameInformationNotificationFactory, ISerializer serializer)
        {
            this.sessionService = sessionService;
            this.gameInformationNotificationFactory = gameInformationNotificationFactory;
            this.serializer = serializer;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.OpenGame;
        }

        public void HandleRequest(GameRequest request)
        {
            var openGameObject = this.serializer.Deserialize<OpenGameRequestObject>(request.SerializedRequestObject);
            var currentSession = this.sessionService.GetByName(openGameObject.SessionName);
            var gameInformationNotificationObject = this.gameInformationNotificationFactory.Create(currentSession);

            this.NotificationService.Send(GameNotificationType.SendGameInformation, gameInformationNotificationObject, openGameObject.PlayerName);
        }
    }
}
