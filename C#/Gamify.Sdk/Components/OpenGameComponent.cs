using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.Components
{
    public class OpenGameComponent : GameComponent
    {
        private readonly ISessionService sessionService;
        private readonly IGameInformationNotificationFactory gameInformationNotificationFactory;
        private readonly ISerializer serializer;

        public OpenGameComponent(ISessionService sessionService, INotificationService notificationService,
            IGameInformationNotificationFactory gameInformationNotificationFactory, ISerializer serializer)
            : base(notificationService)
        {
            this.sessionService = sessionService;
            this.gameInformationNotificationFactory = gameInformationNotificationFactory;
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.OpenGame;
        }

        public override void HandleRequest(GameRequest request)
        {
            var openGameObject = this.serializer.Deserialize<OpenGameRequestObject>(request.SerializedRequestObject);
            var currentSession = this.sessionService.GetByName(openGameObject.SessionName);
            var gameInformationNotificationObject = this.gameInformationNotificationFactory.Create(currentSession);

            this.NotificationService.Send(GameNotificationType.SendGameInformation, gameInformationNotificationObject, openGameObject.PlayerName);
        }
    }
}
