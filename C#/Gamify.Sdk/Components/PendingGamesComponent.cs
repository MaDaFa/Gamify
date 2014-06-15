using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Components
{
    public class PendingGamesComponent : GameComponent
    {
        private readonly ISerializer serializer;
        private readonly ISessionService sessionService;

        public PendingGamesComponent(ISessionService sessionService, INotificationService notificationService,
            ISerializer serializer)
            : base(notificationService)
        {
            this.sessionService = sessionService;
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GetPendingGames;
        }

        public override bool CanHandleNotification(GameNotification notification)
        {
            return notification.Type == (int)GameNotificationType.SendPendingGames;
        }

        public override void HandleRequest(GameRequest request)
        {
            var getPendingGamesObject = this.serializer.Deserialize<GetPendingGamesRequestObject>(request.SerializedRequestObject);
            var pendingPlayerSessions = this.sessionService.GetPendings(getPendingGamesObject.PlayerName);
            var notification = new SendPendingGamesNotificationObject
            {
                PlayerName = getPendingGamesObject.PlayerName
            };

            foreach (var pendingPlayerSession in pendingPlayerSessions)
            {
                notification.AddPendingGame(new GameObject
                {
                    SessionName = pendingPlayerSession.Name,
                    Player1Name = pendingPlayerSession.Player1Name,
                    Player2Name = pendingPlayerSession.Player2Name
                });
            }

            this.notificationService.Send(GameNotificationType.SendPendingGames, notification, getPendingGamesObject.PlayerName);
        }
    }
}
