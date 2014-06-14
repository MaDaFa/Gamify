using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.Components
{
    public class GameMoveComponent<TMove, UResponse> : GameComponent
    {
        private readonly IMoveService<TMove, UResponse> moveService;
        private readonly ISessionService sessionService;
        private readonly ISessionHistoryService<TMove, UResponse> sessionHistoryService;
        private readonly IMoveFactory<TMove> moveFactory;
        private readonly IMoveResultNotificationFactory moveResultNotificationFactory;
        private readonly ISerializer serializer;

        public GameMoveComponent(IMoveService<TMove, UResponse> moveService, ISessionService sessionService,
            ISessionHistoryService<TMove, UResponse> sessionHistoryService, INotificationService notificationService,
            IMoveFactory<TMove> moveFactory, IMoveResultNotificationFactory moveResultNotificationFactory, ISerializer serializer)
            : base(notificationService)
        {
            this.moveService = moveService;
            this.sessionService = sessionService;
            this.sessionHistoryService = sessionHistoryService;
            this.moveFactory = moveFactory;
            this.moveResultNotificationFactory = moveResultNotificationFactory;
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.GameMove;
        }

        public override bool CanHandleNotification(GameNotification notification)
        {
            return notification.Type == (int)GameNotificationType.GameMove ||
                notification.Type == (int)GameNotificationType.GameMoveResult ||
                notification.Type == (int)GameNotificationType.GameFinished;
        }

        public override void HandleRequest(GameRequest request)
        {
            var moveRequestObject = this.serializer.Deserialize<MoveRequestObject>(request.SerializedRequestObject);
            var currentSession = this.sessionService.GetByName(moveRequestObject.SessionName);
            var originPlayer = currentSession.GetPlayer(moveRequestObject.PlayerName);
            var destinationPlayer = currentSession.GetVersusPlayer(originPlayer.Information.Name);
            var move = this.moveFactory.Create(moveRequestObject.MoveInformation);
            var moveResponse = this.moveService.Handle(currentSession.Name, originPlayer.Information.Name, move);

            var sessionHistoryItem = new SessionHistoryItem<TMove, UResponse>(move.MoveObject, moveResponse.MoveResponseObject);

            this.sessionHistoryService.Add(currentSession.Name, originPlayer.Information.Name, sessionHistoryItem);

            if (moveResponse.IsWin)
            {
                this.sessionService.Finish(currentSession.Name);

                var gameFinishedNotificationObject = new GameFinishedNotificationObject
                {
                    SessionName = currentSession.Name,
                    WinnerPlayerName = originPlayer.Information.Name,
                    LooserPlayerName = destinationPlayer.Information.Name
                };

                this.notificationService.SendBroadcast(GameNotificationType.GameFinished, gameFinishedNotificationObject, gameFinishedNotificationObject.WinnerPlayerName, gameFinishedNotificationObject.LooserPlayerName);
            }
            else
            {
                originPlayer.PendingToMove = false;
                destinationPlayer.PendingToMove = true;

                this.SendMoveNotification(moveRequestObject, destinationPlayer.Information.Name);
                this.SendMoveResultNotification(moveRequestObject, moveResponse, destinationPlayer.Information.Name);
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

            this.notificationService.Send(GameNotificationType.GameMove, gameMoveNotificationObject, destinationPlayerName);
        }

        private void SendMoveResultNotification(MoveRequestObject moveRequestObject, IGameMoveResponse moveResponse, string destinationPlayerName)
        {
            var gameMoveResultNotificationObject = this.moveResultNotificationFactory.Create(moveRequestObject, moveResponse);

            this.notificationService.Send(GameNotificationType.GameMoveResult, gameMoveResultNotificationObject, moveRequestObject.PlayerName);
        }
    }
}
