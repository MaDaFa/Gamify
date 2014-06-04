using Gamify.Contracts.Requests;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;

namespace Gamify.Sdk.Components
{
    public class GameMoveComponent : IGameComponent
    {
        private readonly IMoveService moveService;
        private readonly ISessionService sessionService;
        private readonly IMoveHandler moveHandler;
        private readonly IMoveResultNotificationFactory moveResultNotificationFactory;
        private readonly ISerializer serializer;

        public INotificationService NotificationService { get; private set; }

        public GameMoveComponent(IMoveService moveService, ISessionService sessionService, INotificationService notificationService,
            IMoveHandler moveHandler, IMoveResultNotificationFactory moveResultNotificationFactory, ISerializer serializer)
        {
            this.moveService = moveService;
            this.sessionService = sessionService;
            this.moveHandler = moveHandler;
            this.moveResultNotificationFactory = moveResultNotificationFactory;
            this.serializer = serializer;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GameMove;
        }

        public void HandleRequest(GameRequest request)
        {
            var moveRequestObject = this.serializer.Deserialize<MoveRequestObject>(request.SerializedRequestObject);
            var currentSession = this.sessionService.GetByName(moveRequestObject.SessionName);
            var originPlayer = currentSession.GetPlayer(moveRequestObject.PlayerName);
            var destinationPlayer = currentSession.GetVersusPlayer(originPlayer.Information.UserName);
            var moveResponse = this.moveHandler.Handle(moveRequestObject, this.moveService);

            if (moveResponse.IsWin)
            {
                var gameFinishedNotificationObject = new GameFinishedNotificationObject
                {
                    SessionName = currentSession.Name,
                    WinnerPlayerName = originPlayer.Information.UserName,
                    LooserPlayerName = destinationPlayer.Information.UserName
                };

                this.NotificationService.SendBroadcast(GameNotificationType.GameFinished, gameFinishedNotificationObject, gameFinishedNotificationObject.WinnerPlayerName, gameFinishedNotificationObject.LooserPlayerName);

                return;
            }
            else
            {
                originPlayer.PendingToMove = false;
                destinationPlayer.PendingToMove = true;

                this.SendMoveNotification(moveRequestObject, destinationPlayer.Information.UserName);
                this.SendMoveResultNotification(moveRequestObject, moveResponse, destinationPlayer.Information.UserName);
            }
        }

        private void SendMoveNotification(MoveRequestObject moveRequestObject, string destinationPlayerName)
        {
            var gameMoveNotificationObject = new MoveNotificationObject
            {
                SessionName = moveRequestObject.SessionName,
                PlayerName = moveRequestObject.PlayerName,
                MoveInformation = moveRequestObject.MoveInformation
            };

            this.NotificationService.Send(GameNotificationType.GameMove, gameMoveNotificationObject, destinationPlayerName);
        }

        private void SendMoveResultNotification(MoveRequestObject moveRequestObject, IGameMoveResponse moveResponse, string destinationPlayerName)
        {
            var gameMoveResultNotificationObject = this.moveResultNotificationFactory.Create(moveRequestObject, moveResponse);

            this.NotificationService.Send(GameNotificationType.GameMoveResult, gameMoveResultNotificationObject, moveRequestObject.PlayerName);
        }
    }
}
