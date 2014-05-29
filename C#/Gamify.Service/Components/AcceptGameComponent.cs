using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Service.Interfaces;

namespace Gamify.Service.Components
{
    public abstract class AcceptGameComponent : IGameComponent
    {
        protected readonly ISerializer<GameAcceptedRequestObject> serializer;
        protected readonly ISessionService sessionService;

        public INotificationService NotificationService { get; private set; }

        protected AcceptGameComponent(ISessionService sessionService, INotificationService notificationService)
        {
            this.serializer = new JsonSerializer<GameAcceptedRequestObject>();
            this.sessionService = sessionService;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GameAccepted;
        }

        public void HandleRequest(GameRequest request)
        {
            var gameAcceptedObject = this.serializer.Deserialize(request.SerializedRequestObject);

            this.GetSessionPlayer2Ready(gameAcceptedObject);

            var newSession = this.sessionService.GetByName(gameAcceptedObject.SessionName);

            newSession.Player1.PendingToMove = true;

            this.SendGameCreatedNotification(newSession);
        }

        protected abstract void GetSessionPlayer2Ready(GameAcceptedRequestObject gameAcceptedRequestObject);

        private void SendGameCreatedNotification(IGameSession newSession)
        {
            var notification = new GameCreatedNotificationObject
            {
                SessionName = newSession.Name,
                Player1Name = newSession.Player1.Information.UserName,
                Player2Name = newSession.Player2.Information.UserName
            };

            this.NotificationService.SendBroadcast(GameNotificationType.GameCreated, notification, newSession.Player1.Information.UserName, newSession.Player2.Information.UserName);
        }
    }
}
