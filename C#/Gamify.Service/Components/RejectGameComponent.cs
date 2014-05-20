using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Core.Interfaces;
using System.Linq;

namespace Gamify.Service.Components
{
    public class RejectGameComponent : IGameComponent
    {
        private readonly ISerializer<GameRejectedRequestObject> serializer;
        private readonly INotificationService notificationService;
        private readonly IGameController gameController;

        public RejectGameComponent(INotificationService notificationService, IGameController gameController)
        {
            this.serializer = new JsonSerializer<GameRejectedRequestObject>();
            this.notificationService = notificationService;
            this.gameController = gameController;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GameRejected;
        }

        public void HandleRequest(GameRequest request)
        {
            var gameRejectedObject = this.serializer.Deserialize(request.SerializedRequestObject);
            var newSession = this.gameController.Sessions.First(s => s.Id == gameRejectedObject.SessionId);
            var notification = new GameRejectedNotificationObject
            {
                SessionId = newSession.Id,
                Player1Name = newSession.Player1.Information.UserName,
                Player2Name = newSession.Player2.Information.UserName
            };

            this.notificationService.Send(GameNotificationType.GameRejected, notification, newSession.Player1.Information.UserName);
        }
    }
}
