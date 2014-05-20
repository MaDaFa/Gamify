using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Core.Interfaces;
using System.Linq;

namespace Gamify.Service.Components
{
    public class AbandonGameComponent : IGameComponent
    {
        private readonly ISerializer<AbandonGameRequestObject> serializer;
        private readonly INotificationService notificationService;
        private readonly IGameController gameController;

        public AbandonGameComponent(INotificationService notificationService, IGameController gameController)
        {
            this.serializer = new JsonSerializer<AbandonGameRequestObject>();
            this.notificationService = notificationService;
            this.gameController = gameController;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.AbandonGame;
        }

        public void HandleRequest(GameRequest request)
        {
            var abandonGameObject = this.serializer.Deserialize(request.SerializedRequestObject);
            var currentSession = this.gameController.Sessions.First(s => s.Id == abandonGameObject.SessionId);

            this.gameController.AbandonSession(abandonGameObject.PlayerName, abandonGameObject.SessionId);

            this.SendAbandonGameNotification(abandonGameObject, currentSession);
        }

        private void SendAbandonGameNotification(AbandonGameRequestObject abandonGameObject, IGameSession currentSession)
        {
            var notification = new GameAbandonedNotificationObject
            {
                SessionId = abandonGameObject.SessionId,
                PlayerName = abandonGameObject.PlayerName
            };

            this.notificationService.SendBroadcast(GameNotificationType.GameAbandoned, notification, currentSession.Player1.Information.UserName, currentSession.Player2.Information.UserName);
        }
    }
}
