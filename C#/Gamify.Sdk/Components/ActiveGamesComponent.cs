using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Components
{
    public class ActiveGamesComponent : GameComponent
    {
        private readonly ISerializer serializer;
        private readonly ISessionService sessionService;

        public ActiveGamesComponent(ISessionService sessionService, INotificationService notificationService,
            ISerializer serializer)
            : base(notificationService)
        {
            this.sessionService = sessionService;
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GetActiveGames;
        }

        public override bool CanHandleNotification(GameNotification notification)
        {
            return notification.Type == (int)GameNotificationType.SendActiveGames;
        }

        public override void HandleRequest(GameRequest request)
        {
            var getActiveGamesObject = this.serializer.Deserialize<GetActiveGamesRequestObject>(request.SerializedRequestObject);
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
                    Player1Name = activePlayerSession.Player1Name,
                    Player2Name = activePlayerSession.Player2Name
                });
            }

            this.notificationService.Send(GameNotificationType.SendActiveGames, notification, getActiveGamesObject.PlayerName);
        }
    }
}
