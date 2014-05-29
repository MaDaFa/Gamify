using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Service.Interfaces;
using System.Linq;

namespace Gamify.Service.Components
{
    public class ConnectPlayerComponent : IGameComponent
    {
        private readonly ISerializer<PlayerConnectRequestObject> serializer;
        private readonly IPlayerService playerService;

        public INotificationService NotificationService { get; private set; }

        public ConnectPlayerComponent(IPlayerService playerService, INotificationService notificationService)
        {
            this.serializer = new JsonSerializer<PlayerConnectRequestObject>();
            this.playerService = playerService;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.PlayerConnect;
        }

        public void HandleRequest(GameRequest request)
        {
            var playerConnectObject = this.serializer.Deserialize(request.SerializedRequestObject);

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
