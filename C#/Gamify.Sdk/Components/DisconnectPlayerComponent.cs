using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;
using System.Linq;

namespace Gamify.Sdk.Components
{
    public class DisconnectPlayerComponent : GameComponent
    {
        private readonly IPlayerService playerService;
        private readonly ISerializer serializer;

        public DisconnectPlayerComponent(IPlayerService playerService, INotificationService notificationService,
            ISerializer serializer)
            : base(notificationService)
        {
            this.playerService = playerService;
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.PlayerDisconnect;
        }

        public override void HandleRequest(GameRequest request)
        {
            var playerDisconnectObject = this.serializer.Deserialize<PlayerDisconnectRequestObject>(request.SerializedRequestObject);

            this.playerService.Disconnect(playerDisconnectObject.PlayerName);

            this.SendPlayerDisconnectedNotification(playerDisconnectObject.PlayerName);
        }

        private void SendPlayerDisconnectedNotification(string userName)
        {
            var notification = new PlayerDisconnectedNotificationObject
            {
                PlayerName = userName
            };
            var players = this.playerService.GetAllConnected(playerNameToExclude: userName)
                .Select(p => p.Name);

            this.NotificationService.SendBroadcast(GameNotificationType.PlayerDisconnected, notification, players.ToArray());
        }
    }
}
