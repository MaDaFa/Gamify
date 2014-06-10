using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;
using System.Linq;

namespace Gamify.Sdk.Components
{
    public class ConnectedPlayersComponent : GameComponent
    {
        private readonly IPlayerService playerService;
        private readonly ISerializer serializer;

        public ConnectedPlayersComponent(IPlayerService playerService, INotificationService notificationService, ISerializer serializer)
            : base(notificationService)
        {
            this.playerService = playerService;
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GetConnectedPlayers;
        }

        public override void HandleRequest(GameRequest request)
        {
            var getConnectedPlayersObject = this.serializer.Deserialize<GetConnectedPlayersRequestObject>(request.SerializedRequestObject);
            var sortedPlayers = this.playerService.GetAllConnected(playerNameToExclude: getConnectedPlayersObject.PlayerName)
                .OrderBy(p => p.Name);
            var sortedPlayersPage = sortedPlayers.Take(getConnectedPlayersObject.PageSize);
            var notification = new SendConnectedPlayersNotificationObject
            {
                PlayerName = getConnectedPlayersObject.PlayerName,
                ConnectedPlayerNames = sortedPlayersPage.Select(p => p.Name),
                ConectedPlayersCount = sortedPlayers.Count()
            };

            this.NotificationService.Send(GameNotificationType.SendConnectedPlayers, notification, getConnectedPlayersObject.PlayerName);
        }
    }
}
