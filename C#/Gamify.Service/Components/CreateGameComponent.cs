using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Service.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Service.Components
{
    public abstract class CreateGameComponent : IGameComponent
    {
        protected readonly ISerializer<CreateGameRequestObject> serializer;
        protected readonly ISessionService sessionService;

        public INotificationService NotificationService { get; private set; }

        protected CreateGameComponent(ISessionService sessionService, INotificationService notificationService)
        {
            this.serializer = new JsonSerializer<CreateGameRequestObject>();
            this.sessionService = sessionService;

            this.NotificationService = notificationService;
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
            var newSession = this.sessionService.Open(sessionPlayer1, sessionPlayer2);

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
                SessionName = newSession.Name,
                Player1Name = newSession.Player1.Information.UserName
            };

            this.DecorateGameInvitation(gameInviteNotificationObject);

            this.NotificationService.Send(GameNotificationType.GameInvite, gameInviteNotificationObject, newSession.Player2.Information.UserName);
        }
    }
}
