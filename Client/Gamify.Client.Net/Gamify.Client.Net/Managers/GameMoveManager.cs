using Gamify.Client.Net.Client;
using Gamify.Client.Net.Services;
using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using System;

namespace Gamify.Client.Net.Managers
{
    public class GameMoveManager<TGameMoveRequest, TMoveResultNotification, TMoveNotification>
        where TGameMoveRequest : IRequestObject
        where TMoveResultNotification : INotificationObject
        where TMoveNotification : INotificationObject
    {
        private readonly IGameService<TGameMoveRequest, TMoveResultNotification> gameMoveService;
        private readonly IGameListener<TMoveNotification> gameMoveListener;

        public event EventHandler<GameNotificationEventArgs<TMoveResultNotification>> GameMoveResultNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<TMoveNotification>> GameMoveNotificationReceived;

        public GameMoveManager(string playerName, IGameClientFactory clientFactory)
        {
            var gameClient = clientFactory.GetGameClient(playerName);

            this.gameMoveService = new GameService<TGameMoveRequest, TMoveResultNotification>(GameRequestType.GameMove, GameNotificationType.GameMoveResult, gameClient);
            this.gameMoveListener = new GameListener<TMoveNotification>(GameNotificationType.GameMove, gameClient);

            this.gameMoveService.NotificationReceived += (sender, args) =>
            {
                this.NotifyGameMoveResult(args);
            };
            this.gameMoveListener.NotificationReceived += (sender, args) =>
            {
                this.NotifyGameMove(args);
            };
        }

        public void SendMove(TGameMoveRequest gameMoveRequest)
        {
            this.gameMoveService.Send(gameMoveRequest);
        }

        private void NotifyGameMoveResult(GameNotificationEventArgs<TMoveResultNotification> args)
        {
            if (this.GameMoveResultNotificationReceived != null)
            {
                this.GameMoveResultNotificationReceived(this, args);
            }
        }

        private void NotifyGameMove(GameNotificationEventArgs<TMoveNotification> args)
        {
            if (this.GameMoveNotificationReceived != null)
            {
                this.GameMoveNotificationReceived(this, args);
            }
        }
    }
}
