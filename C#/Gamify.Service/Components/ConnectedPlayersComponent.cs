using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Service.Interfaces;
using System.Linq;

namespace Gamify.Service.Components
{
    public class ConnectedPlayersComponent : IGameComponent
    {
        private readonly ISerializer<GetConnectedPlayersRequestObject> serializer;
        private readonly IPlayerService playerService;

        public INotificationService NotificationService { get; private set; }

        public ConnectedPlayersComponent(IPlayerService playerService, INotificationService notificationService)
        {
            this.serializer = new JsonSerializer<GetConnectedPlayersRequestObject>();
            this.playerService = playerService;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GetConnectedPlayers;
        }

        public void HandleRequest(GameRequest request)
        {
            var getConnectedPlayersObject = this.serializer.Deserialize(request.SerializedRequestObject);
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
