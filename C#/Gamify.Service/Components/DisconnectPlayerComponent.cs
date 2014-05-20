using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Core.Interfaces;
using System.Linq;

namespace Gamify.Service.Components
{
    public class DisconnectPlayerComponent : IGameComponent
    {
        private readonly ISerializer<PlayerDisconnectRequestObject> serializer;
        private readonly INotificationService notificationService;
        private readonly IGameController gameController;

        public DisconnectPlayerComponent(INotificationService notificationService, IGameController gameController)
        {
            this.serializer = new JsonSerializer<PlayerDisconnectRequestObject>();
            this.notificationService = notificationService;
            this.gameController = gameController;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.PlayerDisconnect;
        }

        public void HandleRequest(GameRequest request)
        {
            var playerDisconnectObject = this.serializer.Deserialize(request.SerializedRequestObject);

            this.gameController.Disconnect(playerDisconnectObject.PlayerName);
            this.SendPlayerDisconnectedNotification(playerDisconnectObject.PlayerName);
        }

        private void SendPlayerDisconnectedNotification(string userName)
        {
            var notification = new PlayerDisconnectedNotificationObject
            {
                PlayerName = userName
            };
            var players = this.gameController.Players
                .Where(p => p.UserName != userName)
                .Select(p => p.UserName);

            this.notificationService.SendBroadcast(GameNotificationType.PlayerDisconnected, notification, players.ToArray());
        }
    }
}
