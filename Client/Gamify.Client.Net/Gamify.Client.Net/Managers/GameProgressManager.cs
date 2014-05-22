using Gamify.Client.Net.Client;
using Gamify.Client.Net.Services;
using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using System;

namespace Gamify.Client.Net.Managers
{
    public class GameProgressManager<TGameInformationNotification> where TGameInformationNotification : INotificationObject
    {
        private readonly IGameService<OpenGameRequestObject, TGameInformationNotification> openGameService;
        private readonly IGameService<GetActiveGamesRequestObject, SendActiveGamesNotificationObject> activeGamesService;
        private readonly IGameService<AbandonGameRequestObject, GameAbandonedNotificationObject> abandonGameService;
        private readonly IGameListener<GameFinishedNotificationObject> gameFinishedListener;

        public event EventHandler<GameNotificationEventArgs<TGameInformationNotification>> GameInformationNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<SendActiveGamesNotificationObject>> ActiveGamesNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<GameAbandonedNotificationObject>> GameAbandonedNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<GameFinishedNotificationObject>> GameFinishedNotificationReceived;

        public GameProgressManager(string playerName, IGameClientFactory clientFactory)
        {
            var gameClient = clientFactory.GetGameClient(playerName);

            this.openGameService = new GameService<OpenGameRequestObject, TGameInformationNotification>(GameRequestType.OpenGame, GameNotificationType.SendGameInformation, gameClient);
            this.activeGamesService = new GameService<GetActiveGamesRequestObject, SendActiveGamesNotificationObject>(GameRequestType.GetActiveGames, GameNotificationType.SendActiveGames, gameClient);
            this.abandonGameService = new GameService<AbandonGameRequestObject, GameAbandonedNotificationObject>(GameRequestType.AbandonGame, GameNotificationType.GameAbandoned, gameClient);
            this.gameFinishedListener = new GameListener<GameFinishedNotificationObject>(GameNotificationType.GameFinished, gameClient);

            this.openGameService.NotificationReceived += (sender, args) =>
            {
                this.NotifyGameInformation(args);
            };
            this.activeGamesService.NotificationReceived += (sender, args) =>
            {
                this.NotifyActiveGames(args);
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

        public void OpenGame(OpenGameRequestObject openGameRequest)
        {
            this.openGameService.Send(openGameRequest);
        }

        public void GetActiveGames(GetActiveGamesRequestObject getActiveGamesRequest)
        {
            this.activeGamesService.Send(getActiveGamesRequest);
        }

        public void AbandonGame(AbandonGameRequestObject abandonGameRequest)
        {
            this.abandonGameService.Send(abandonGameRequest);
        }

        private void NotifyGameInformation(GameNotificationEventArgs<TGameInformationNotification> args)
        {
            if (this.GameInformationNotificationReceived != null)
            {
                this.GameInformationNotificationReceived(this, args);
            }
        }

        private void NotifyActiveGames(GameNotificationEventArgs<SendActiveGamesNotificationObject> args)
        {
            if (this.ActiveGamesNotificationReceived != null)
            {
                this.ActiveGamesNotificationReceived(this, args);
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
