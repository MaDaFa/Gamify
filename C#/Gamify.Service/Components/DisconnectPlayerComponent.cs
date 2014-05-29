using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Service.Interfaces;
using System.Linq;

namespace Gamify.Service.Components
{
    public class DisconnectPlayerComponent : IGameComponent
    {
        private readonly ISerializer<PlayerDisconnectRequestObject> serializer;
        private readonly IPlayerService playerService;

        public INotificationService NotificationService { get; private set; }

        public DisconnectPlayerComponent(IPlayerService playerService, INotificationService notificationService)
        {
            this.serializer = new JsonSerializer<PlayerDisconnectRequestObject>();
            this.playerService = playerService;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.PlayerDisconnect;
        }

        public void HandleRequest(GameRequest request)
        {
            var playerDisconnectObject = this.serializer.Deserialize(request.SerializedRequestObject);

            this.SendPlayerDisconnectedNotification(playerDisconnectObject.PlayerName);
        }

        private void SendPlayerDisconnectedNotification(string userName)
        {
            var notification = new PlayerDisconnectedNotificationObject
            {
                PlayerName = userName
            };
            var players = this.playerService.GetAll(playerNameToExclude: userName)
                .Select(p => p.UserName);

            this.NotificationService.SendBroadcast(GameNotificationType.PlayerDisconnected, notification, players.ToArray());
        }
    }
}
