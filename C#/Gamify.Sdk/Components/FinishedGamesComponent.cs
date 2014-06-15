using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Components
{
    public class FinishedGamesComponent : GameComponent
    {
        private readonly ISerializer serializer;
        private readonly ISessionService sessionService;

        public FinishedGamesComponent(ISessionService sessionService, INotificationService notificationService,
            ISerializer serializer)
            : base(notificationService)
        {
            this.sessionService = sessionService;
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GetFinishedGames;
        }

        public override bool CanHandleNotification(GameNotification notification)
        {
            return notification.Type == (int)GameNotificationType.SendFinishedGames;
        }

        public override void HandleRequest(GameRequest request)
        {
            var getFinishedGamesObject = this.serializer.Deserialize<GetFinishedGamesRequestObject>(request.SerializedRequestObject);
            var finishedPlayerSessions = this.sessionService.GetFinished(getFinishedGamesObject.PlayerName);
            var notification = new SendFinishedGamesNotificationObject
            {
                PlayerName = getFinishedGamesObject.PlayerName
            };

            foreach (var finishedPlayerSession in finishedPlayerSessions)
            {
                notification.AddFinishedGame(new GameObject
                {
                    SessionName = finishedPlayerSession.Name,
                    Player1Name = finishedPlayerSession.Player1Name,
                    Player2Name = finishedPlayerSession.Player2Name
                });
            }

            this.notificationService.Send(GameNotificationType.SendFinishedGames, notification, getFinishedGamesObject.PlayerName);
        }
    }
}
