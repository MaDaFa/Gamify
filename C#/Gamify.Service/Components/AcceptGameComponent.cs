using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Core.Interfaces;
using System.Linq;

namespace Gamify.Service.Components
{
    public abstract class AcceptGameComponent : IGameComponent
    {
        private readonly ISerializer<GameAcceptedRequestObject> serializer;
        private readonly INotificationService notificationService;
        private readonly IGameController gameController;

        protected AcceptGameComponent(INotificationService notificationService, IGameController gameController)
        {
            this.serializer = new JsonSerializer<GameAcceptedRequestObject>();
            this.notificationService = notificationService;
            this.gameController = gameController;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GameAccepted;
        }

        public void HandleRequest(GameRequest request)
        {
            var gameAcceptedObject = this.serializer.Deserialize(request.SerializedRequestObject);

            this.GetSessionPlayer2Ready(gameAcceptedObject);

            var newSession = this.gameController.Sessions.First(s => s.Id == gameAcceptedObject.SessionId);

            newSession.Player1.NeedsToMove = true;

            this.SendGameCreatedNotification(newSession);
        }

        protected abstract void GetSessionPlayer2Ready(GameAcceptedRequestObject gameAcceptedRequestObject);

        private void SendGameCreatedNotification(IGameSession newSession)
        {
            var notification = new GameCreatedNotificationObject
            {
                SessionId = newSession.Id,
                Player1Name = newSession.Player1.Information.UserName,
                Player2Name = newSession.Player2.Information.UserName
            };

            this.notificationService.SendBroadcast(GameNotificationType.GameCreated, notification, newSession.Player1.Information.UserName, newSession.Player2.Information.UserName);
        }
    }
}
