using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Service.Components
{
    public abstract class CreateGameComponent : IGameComponent
    {
        private readonly ISerializer<CreateGameRequestObject> serializer;
        private readonly INotificationService notificationService;
        private readonly IGameController gameController;

        protected CreateGameComponent(INotificationService notificationService, IGameController gameController)
        {
            this.serializer = new JsonSerializer<CreateGameRequestObject>();
            this.notificationService = notificationService;
            this.gameController = gameController;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.CreateGame;
        }

        public void HandleRequest(GameRequest request)
        {
            var createGameObject = this.serializer.Deserialize(request.SerializedRequestObject);
            var sessionPlayers = this.GetSessionPlayers(createGameObject);
            var sessionPlayer1 = sessionPlayers.First(p => p.Information.UserName == createGameObject.PlayerName);
            var sessionPlayer2 = sessionPlayers.First(p => p.Information.UserName == createGameObject.InvitedPlayerName);
            var newSession = this.gameController.OpenSession(sessionPlayer1, sessionPlayer2);

            this.SendGameInviteNotification(newSession);
        }

        protected abstract IEnumerable<ISessionGamePlayerBase> GetSessionPlayers(CreateGameRequestObject createGameRequestObject);

        protected virtual void DecorateGameInvitation(GameInviteNotificationObject gameInviteNotificationObject)
        {
        }

        private void SendGameInviteNotification(IGameSession newSession)
        {
            var gameInviteNotificationObject = new GameInviteNotificationObject
            {
                SessionId = newSession.Id,
                Player1Name = newSession.Player1.Information.UserName
            };

            this.DecorateGameInvitation(gameInviteNotificationObject);

            this.notificationService.Send(GameNotificationType.GameInvite, gameInviteNotificationObject, newSession.Player2.Information.UserName);
        }
    }
}
