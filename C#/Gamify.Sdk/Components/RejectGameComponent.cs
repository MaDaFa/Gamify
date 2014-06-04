using Gamify.Contracts.Requests;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Components
{
    public class RejectGameComponent : IGameComponent
    {
        private readonly ISessionService sessionService;
        private readonly ISerializer serializer;

        public INotificationService NotificationService { get; private set; }

        public RejectGameComponent(ISessionService sessionService, INotificationService notificationService,
            ISerializer serializer)
        {
            this.sessionService = sessionService;
            this.serializer = serializer;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GameRejected;
        }

        public void HandleRequest(GameRequest request)
        {
            var gameRejectedObject = this.serializer.Deserialize<GameRejectedRequestObject>(request.SerializedRequestObject);
            var newSession = this.sessionService.GetByName(gameRejectedObject.SessionName);
            var notification = new GameRejectedNotificationObject
            {
                SessionName = newSession.Name,
                Player1Name = newSession.Player1.Information.UserName,
                Player2Name = newSession.Player2.Information.UserName
            };

            this.NotificationService.Send(GameNotificationType.GameRejected, notification, newSession.Player1.Information.UserName);
        }
    }
}
