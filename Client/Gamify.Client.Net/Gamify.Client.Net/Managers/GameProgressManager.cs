using Gamify.Client.Net.Client;
using Gamify.Client.Net.Contracts.Notifications;
using Gamify.Client.Net.Contracts.Requests;
using Gamify.Client.Net.Services;
using System;

namespace Gamify.Client.Net.Managers
{
    public class GameProgressManager<TMoveResultNotification> where TMoveResultNotification : IMoveResultNotificationObject
    {
        private readonly IGameService<MoveRequestObject, TMoveResultNotification> gameMoveService;
        private readonly IGameListener<MoveNotificationObject> gameMoveListener;
        private readonly IGameService<AbandonGameRequestObject, GameAbandonedNotificationObject> abandonGameService;
        private readonly IGameListener<GameFinishedNotificationObject> gameFinishedListener;

        public event EventHandler<GameNotificationEventArgs<TMoveResultNotification>> GameMoveResultNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<MoveNotificationObject>> GameMoveNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<GameAbandonedNotificationObject>> GameAbandonedNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<GameFinishedNotificationObject>> GameFinishedNotificationReceived;

        public GameProgressManager(IGameClientFactory clientFactory)
        {
            var gameClient = clientFactory.Create();

            this.gameMoveService = new GameService<MoveRequestObject, TMoveResultNotification>(GameRequestType.GameMove, GameNotificationType.GameMoveResult, gameClient);
            this.gameMoveListener = new GameListener<MoveNotificationObject>(GameNotificationType.GameMove, gameClient);
            this.abandonGameService = new GameService<AbandonGameRequestObject, GameAbandonedNotificationObject>(GameRequestType.AbandonGame, GameNotificationType.GameAbandoned, gameClient);
            this.gameFinishedListener = new GameListener<GameFinishedNotificationObject>(GameNotificationType.GameFinished, gameClient);

            this.gameMoveService.NotificationReceived += (sender, args) =>
            {
                this.NotifyGameMoveResult(args);
            };
            this.gameMoveListener.NotificationReceived += (sender, args) =>
            {
                this.NotifyGameMove(args);
            };
            this.abandonGameService.NotificationReceived += (sender, args) =>
            {
                this.NotifyGameAbandoned(args);
            };
            this.gameFinishedListener.NotificationReceived += (sender, args) =>
            {
                this.NotifyGameFinished(args);
            };
        }

        public void SendMove(MoveRequestObject gameMoveRequest)
        {
            this.gameMoveService.Send(gameMoveRequest);
        }

        public void AbandonGame(AbandonGameRequestObject abandonGameRequest)
        {
            this.abandonGameService.Send(abandonGameRequest);
        }

        private void NotifyGameMoveResult(GameNotificationEventArgs<TMoveResultNotification> args)
        {
            if (this.GameMoveResultNotificationReceived != null)
            {
                this.GameMoveResultNotificationReceived(this, args);
            }
        }

        private void NotifyGameMove(GameNotificationEventArgs<MoveNotificationObject> args)
        {
            if (this.GameMoveNotificationReceived != null)
            {
                this.GameMoveNotificationReceived(this, args);
            }
        }

        private void NotifyGameAbandoned(GameNotificationEventArgs<GameAbandonedNotificationObject> args)
        {
            if (this.GameAbandonedNotificationReceived != null)
            {
                this.GameAbandonedNotificationReceived(this, args);
            }
        }

        private void NotifyGameFinished(GameNotificationEventArgs<GameFinishedNotificationObject> args)
        {
            if (this.GameFinishedNotificationReceived != null)
            {
                this.GameFinishedNotificationReceived(this, args);
            }
        }
    }
}
