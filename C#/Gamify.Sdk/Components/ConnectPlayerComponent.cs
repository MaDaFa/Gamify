using Gamify.Contracts.Requests;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Services;
using System.Linq;

namespace Gamify.Sdk.Components
{
    public class ConnectPlayerComponent : IGameComponent
    {
        private readonly IPlayerService playerService;
        private readonly ISerializer serializer;

        public INotificationService NotificationService { get; private set; }

        public ConnectPlayerComponent(IPlayerService playerService, INotificationService notificationService, ISerializer serializer)
        {
            this.playerService = playerService;
            this.serializer = serializer;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.PlayerConnect;
        }

        public void HandleRequest(GameRequest request)
        {
            var playerConnectObject = this.serializer.Deserialize<PlayerConnectRequestObject>(request.SerializedRequestObject);

            if (!this.playerService.Exist(playerConnectObject.PlayerName))
            {
                this.playerService.Create(playerConnectObject.PlayerName, playerConnectObject.PlayerName);
            }

            var notification = new PlayerConnectedNotificationObject
            {
                PlayerName = playerConnectObject.PlayerName
            };
            var playersToNotify = this.playerService.GetAll(playerNameToExclude: playerConnectObject.PlayerName)
                .Select(p => p.UserName);

            this.NotificationService.SendBroadcast(GameNotificationType.PlayerConnected, notification, playersToNotify.ToArray());
        }
    }
}
