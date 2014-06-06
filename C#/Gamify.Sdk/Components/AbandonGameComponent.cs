using Gamify.Contracts.Requests;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Components
{
    public class AbandonGameComponent : GameComponent
    {
        private readonly ISessionService sessionService;
        private readonly ISerializer serializer;

        public AbandonGameComponent(ISessionService sessionService, INotificationService notificationService, 
            ISerializer serializer)
            : base(notificationService)
        {
            this.sessionService = sessionService;
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.AbandonGame;
        }

        public override void HandleRequest(GameRequest request)
        {
            var abandonGameObject = this.serializer.Deserialize<AbandonGameRequestObject>(request.SerializedRequestObject);
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
