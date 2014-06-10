using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Components
{
    public class RejectGameComponent : GameComponent
    {
        private readonly ISessionService sessionService;
        private readonly ISerializer serializer;

        public RejectGameComponent(ISessionService sessionService, INotificationService notificationService,
            ISerializer serializer)
            : base(notificationService)
        {
            this.sessionService = sessionService;
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GameRejected;
        }

        public override bool CanHandleNotification(GameNotification notification)
        {
            return notification.Type == (int)GameNotificationType.GameRejected;
        }

        public override void HandleRequest(GameRequest request)
        {
            var gameRejectedObject = this.serializer.Deserialize<GameRejectedRequestObject>(request.SerializedRequestObject);
            var newSession = this.sessionService.GetByName(gameRejectedObject.SessionName);
            var notification = new GameRejectedNotificationObject
            {
                SessionName = newSession.Name,
                Player1Name = newSession.Player1Name,
                Player2Name = newSession.Player2Name
            };

            this.notificationService.Send(GameNotificationType.GameRejected, notification, newSession.Player1Name);
        }
    }
}
