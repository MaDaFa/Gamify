using Gamify.Contracts.Requests;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Services;
using System.Linq;

namespace Gamify.Sdk.Components
{
    public class DisconnectPlayerComponent : IGameComponent
    {
        private readonly IPlayerService playerService;
        private readonly ISerializer serializer;

        public INotificationService NotificationService { get; private set; }

        public DisconnectPlayerComponent(IPlayerService playerService, INotificationService notificationService,
            ISerializer serializer)
        {
            this.playerService = playerService;
            this.serializer = serializer;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.PlayerDisconnect;
        }

        public void HandleRequest(GameRequest request)
        {
            var playerDisconnectObject = this.serializer.Deserialize<PlayerDisconnectRequestObject>(request.SerializedRequestObject);

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
