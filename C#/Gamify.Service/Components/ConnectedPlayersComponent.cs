using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Core.Interfaces;
using System.Linq;

namespace Gamify.Service.Components
{
    public class ConnectedPlayersComponent : IGameComponent
    {
        private readonly ISerializer<GetConnectedPlayersRequestObject> serializer;
        private readonly INotificationService notificationService;
        private readonly IGameController gameController;

        public ConnectedPlayersComponent(INotificationService notificationService, IGameController gameController)
        {
            this.serializer = new JsonSerializer<GetConnectedPlayersRequestObject>();
            this.notificationService = notificationService;
            this.gameController = gameController;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GetConnectedPlayers;
        }

        public void HandleRequest(GameRequest request)
        {
            var getConnectedPlayersObject = this.serializer.Deserialize(request.SerializedRequestObject);
            var sortedPlayers = this.gameController.Players
                .Where(p => p.UserName != getConnectedPlayersObject.PlayerName)
                .OrderBy(p => p.UserName);
            var sortedPlayersPage = sortedPlayers.Take(getConnectedPlayersObject.PageSize);
            var notification = new SendConnectedPlayersNotificationObject
            {
                PlayerName = getConnectedPlayersObject.PlayerName,
                ConnectedPlayerNames = sortedPlayersPage.Select(p => p.UserName),
                ConectedPlayersCount = sortedPlayers.Count()
            };

            this.notificationService.Send(GameNotificationType.SendConnectedPlayers, notification, getConnectedPlayersObject.PlayerName);
        }
    }
}
