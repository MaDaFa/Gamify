using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Service.Interfaces;

namespace Gamify.Service.Components
{
    public class ActiveGamesComponent : IGameComponent
    {
        private readonly ISerializer<GetActiveGamesRequestObject> serializer;
        private readonly ISessionService sessionService;

        public INotificationService NotificationService { get; private set; }

        public ActiveGamesComponent(ISessionService sessionService, INotificationService notificationService)
        {
            this.serializer = new JsonSerializer<GetActiveGamesRequestObject>();
            this.sessionService = sessionService;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GetActiveGames;
        }

        public void HandleRequest(GameRequest request)
        {
            var getActiveGamesObject = this.serializer.Deserialize(request.SerializedRequestObject);
            var activePlayerSessions = this.sessionService.GetAllByPlayer(getActiveGamesObject.PlayerName);
            var notification = new SendActiveGamesNotificationObject
            {
                PlayerName = getActiveGamesObject.PlayerName
            };

            foreach (var activePlayerSession in activePlayerSessions)
            {
                notification.AddActiveGame(new GameObject
                {
                    SessionName = activePlayerSession.Name,
                    Player1Name = activePlayerSession.Player1.Information.UserName,
                    Player2Name = activePlayerSession.Player2.Information.UserName
                });
            }

            this.NotificationService.Send(GameNotificationType.SendActiveGames, notification, getActiveGamesObject.PlayerName);
        }
    }
}
