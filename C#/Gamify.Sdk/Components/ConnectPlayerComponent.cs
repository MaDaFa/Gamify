using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;
using System.Linq;

namespace Gamify.Sdk.Components
{
    public class ConnectPlayerComponent : GameComponent
    {
        private readonly IPlayerService playerService;
        private readonly ISerializer serializer;

        public ConnectPlayerComponent(IPlayerService playerService, INotificationService notificationService, ISerializer serializer)
            : base(notificationService)
        {
            this.playerService = playerService;
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.PlayerConnect;
        }

        public override void HandleRequest(GameRequest request)
        {
            var playerConnectObject = this.serializer.Deserialize<PlayerConnectRequestObject>(request.SerializedRequestObject);

            this.playerService.Connect(playerConnectObject.PlayerName);

            var notification = new PlayerConnectedNotificationObject
            {
                PlayerName = playerConnectObject.PlayerName
            };
            var playersToNotify = this.playerService.GetAllConnected(playerNameToExclude: playerConnectObject.PlayerName)
                .Select(p => p.Name);

            this.NotificationService.SendBroadcast(GameNotificationType.PlayerConnected, notification, playersToNotify.ToArray());
        }
    }
}
