using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Core.Interfaces;
using System.Linq;

namespace Gamify.Service.Components
{
    public class ActiveGamesComponent : IGameComponent
    {
        private readonly ISerializer<GetActiveGamesRequestObject> serializer;
        private readonly INotificationService notificationService;
        private readonly IGameController gameController;

        public ActiveGamesComponent(INotificationService notificationService, IGameController gameController)
        {
            this.serializer = new JsonSerializer<GetActiveGamesRequestObject>();
            this.notificationService = notificationService;
            this.gameController = gameController;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GetActiveGames;
        }

        public void HandleRequest(GameRequest request)
        {
            var getActiveGamesObject = this.serializer.Deserialize(request.SerializedRequestObject);
            var activePlayerGames = this.gameController.Sessions.Where(s => s.HasPlayer(getActiveGamesObject.PlayerName));
            var notification = new SendActiveGamesNotificationObject
            {
                PlayerName = getActiveGamesObject.PlayerName
            };

            foreach (var activePlayerGame in activePlayerGames)
            {
                notification.AddActiveGame(new GameObject
                {
                    SessionId = activePlayerGame.Id,
                    Player1Name = activePlayerGame.Player1.Information.UserName,
                    Player2Name = activePlayerGame.Player2.Information.UserName
                });
            }

            this.notificationService.Send(GameNotificationType.SendActiveGames, notification, getActiveGamesObject.PlayerName);
        }
    }
}
