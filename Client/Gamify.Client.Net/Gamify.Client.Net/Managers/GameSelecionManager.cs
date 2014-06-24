using Gamify.Client.Net.Client;
using Gamify.Client.Net.Contracts.Notifications;
using Gamify.Client.Net.Contracts.Requests;
using Gamify.Client.Net.Services;
using System;

namespace Gamify.Client.Net.Managers
{
    public class GameSelecionManager
    {
        private readonly IGameService<GetActiveGamesRequestObject, SendActiveGamesNotificationObject> activeGamesService;
        private readonly IGameService<GetPendingGamesRequestObject, SendPendingGamesNotificationObject> pendingGamesService;
        private readonly IGameService<GetFinishedGamesRequestObject, SendFinishedGamesNotificationObject> finishedGamesService;
        private readonly IGameService<OpenGameRequestObject, GameInformationNotificationObject> openGameService;

        public event EventHandler<GameNotificationEventArgs<SendActiveGamesNotificationObject>> ActiveGamesNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<SendPendingGamesNotificationObject>> PendingGamesNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<SendFinishedGamesNotificationObject>> FinishedGamesNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<GameInformationNotificationObject>> GameInformationNotificationReceived;

        public GameSelecionManager(IGameClientFactory clientFactory)
        {
            var gameClient = clientFactory.Create();

            this.activeGamesService = new GameService<GetActiveGamesRequestObject, SendActiveGamesNotificationObject>(GameRequestType.GetActiveGames, GameNotificationType.SendActiveGames, gameClient);
            this.pendingGamesService = new GameService<GetPendingGamesRequestObject, SendPendingGamesNotificationObject>(GameRequestType.GetPendingGames, GameNotificationType.SendPendingGames, gameClient);
            this.finishedGamesService = new GameService<GetFinishedGamesRequestObject, SendFinishedGamesNotificationObject>(GameRequestType.GetFinishedGames, GameNotificationType.SendFinishedGames, gameClient);
            this.openGameService = new GameService<OpenGameRequestObject, GameInformationNotificationObject>(GameRequestType.OpenGame, GameNotificationType.SendGameInformation, gameClient);

            this.activeGamesService.NotificationReceived += (sender, args) =>
            {
                this.NotifyActiveGames(args);
            };
            this.pendingGamesService.NotificationReceived += (sender, args) =>
            {
                this.NotifyPendingGames(args);
            };
            this.finishedGamesService.NotificationReceived += (sender, args) =>
            {
                this.NotifyFinishedGames(args);
            };
            this.openGameService.NotificationReceived += (sender, args) =>
            {
                this.NotifyGameInformation(args);
            };
        }

        public void GetActiveGames(GetActiveGamesRequestObject getActiveGamesRequest)
        {
            this.activeGamesService.Send(getActiveGamesRequest);
        }

        public void GetPendingGames(GetPendingGamesRequestObject getPendingGamesRequest)
        {
            this.pendingGamesService.Send(getPendingGamesRequest);
        }

        public void GetFinishedGames(GetFinishedGamesRequestObject getFinishedGamesRequest)
        {
            this.finishedGamesService.Send(getFinishedGamesRequest);
        }

        public void OpenGame(OpenGameRequestObject openGameRequest)
        {
            this.openGameService.Send(openGameRequest);
        }

        private void NotifyActiveGames(GameNotificationEventArgs<SendActiveGamesNotificationObject> args)
        {
            if (this.ActiveGamesNotificationReceived != null)
            {
                this.ActiveGamesNotificationReceived(this, args);
            }
        }

        private void NotifyPendingGames(GameNotificationEventArgs<SendPendingGamesNotificationObject> args)
        {
            if (this.PendingGamesNotificationReceived != null)
            {
                this.PendingGamesNotificationReceived(this, args);
            }
        }

        private void NotifyFinishedGames(GameNotificationEventArgs<SendFinishedGamesNotificationObject> args)
        {
            if (this.FinishedGamesNotificationReceived != null)
            {
                this.FinishedGamesNotificationReceived(this, args);
            }
        }

        private void NotifyGameInformation(GameNotificationEventArgs<GameInformationNotificationObject> args)
        {
            if (this.GameInformationNotificationReceived != null)
            {
                this.GameInformationNotificationReceived(this, args);
            }
        }
    }
}
