using Gamify.Contracts.Requests;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;

namespace Gamify.Sdk.Components
{
    public class AcceptGameComponent : IGameComponent
    {
        private readonly ISessionService sessionService;
        private readonly ISessionPlayerSetup playerSetup;
        private readonly ISerializer serializer;

        public INotificationService NotificationService { get; private set; }

        public AcceptGameComponent(ISessionService sessionService, INotificationService notificationService,
            ISessionPlayerSetup playerSetup, ISerializer serializer)
        {
            this.sessionService = sessionService;
            this.playerSetup = playerSetup;
            this.serializer = serializer;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GameAccepted;
        }

        public void HandleRequest(GameRequest request)
        {
            var gameAcceptedObject = this.serializer.Deserialize<GameAcceptedRequestObject>(request.SerializedRequestObject);
            var newSession = this.sessionService.GetByName(gameAcceptedObject.SessionName);

            this.playerSetup.GetPlayerReady(gameAcceptedObject, newSession.Player2);

            newSession.Player1.PendingToMove = true;

            this.SendGameCreatedNotification(newSession);
        }

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
