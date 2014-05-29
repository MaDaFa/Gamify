using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Service.Interfaces;

namespace Gamify.Service.Components
{
    public class RejectGameComponent : IGameComponent
    {
        private readonly ISerializer<GameRejectedRequestObject> serializer;
        private readonly ISessionService sessionService;

        public INotificationService NotificationService { get; private set; }

        public RejectGameComponent(ISessionService sessionService, INotificationService notificationService)
        {
            this.serializer = new JsonSerializer<GameRejectedRequestObject>();
            this.sessionService = sessionService;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GameRejected;
        }

        public void HandleRequest(GameRequest request)
        {
            var gameRejectedObject = this.serializer.Deserialize(request.SerializedRequestObject);
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
