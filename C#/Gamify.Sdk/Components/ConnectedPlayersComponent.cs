using Gamify.Contracts.Requests;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Services;
using System.Linq;

namespace Gamify.Sdk.Components
{
    public class ConnectedPlayersComponent : IGameComponent
    {
        private readonly IPlayerService playerService;
        private readonly ISerializer serializer;

        public INotificationService NotificationService { get; private set; }

        public ConnectedPlayersComponent(IPlayerService playerService, INotificationService notificationService, ISerializer serializer)
        {
            this.playerService = playerService;
            this.serializer = serializer;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GetConnectedPlayers;
        }

        public void HandleRequest(GameRequest request)
        {
            var getConnectedPlayersObject = this.serializer.Deserialize<GetConnectedPlayersRequestObject>(request.SerializedRequestObject);
            var sortedPlayers = this.playerService.GetAll(playerNameToExclude: getConnectedPlayersObject.PlayerName)
                .OrderBy(p => p.UserName);
            var sortedPlayersPage = sortedPlayers.Take(getConnectedPlayersObject.PageSize);
            var notification = new SendConnectedPlayersNotificationObject
            {
                PlayerName = getConnectedPlayersObject.PlayerName,
                ConnectedPlayerNames = sortedPlayersPage.Select(p => p.UserName),
                ConectedPlayersCount = sortedPlayers.Count()
            };

            this.NotificationService.Send(GameNotificationType.SendConnectedPlayers, notification, getConnectedPlayersObject.PlayerName);
        }
    }
}
