using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Service.Interfaces;

namespace Gamify.Service.Components
{
    public class AbandonGameComponent : IGameComponent
    {
        private readonly ISerializer<AbandonGameRequestObject> serializer;
        private readonly ISessionService sessionService;

        public INotificationService NotificationService { get; private set; }

        public AbandonGameComponent(ISessionService sessionService, INotificationService notificationService)
        {
            this.serializer = new JsonSerializer<AbandonGameRequestObject>();
            this.sessionService = sessionService;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.AbandonGame;
        }

        public void HandleRequest(GameRequest request)
        {
            var abandonGameObject = this.serializer.Deserialize(request.SerializedRequestObject);
            var currentSession = this.sessionService.GetByName(abandonGameObject.SessionName);

            this.sessionService.Abandon(currentSession.Name);

            this.SendAbandonGameNotification(abandonGameObject, currentSession);
        }

        private void SendAbandonGameNotification(AbandonGameRequestObject abandonGameObject, IGameSession currentSession)
        {
            var notification = new GameAbandonedNotificationObject
            {
                SessionName = abandonGameObject.SessionName,
                PlayerName = abandonGameObject.PlayerName
            };

            this.NotificationService.SendBroadcast(GameNotificationType.GameAbandoned, notification, currentSession.Player1.Information.UserName, currentSession.Player2.Information.UserName);
        }
    }
}
