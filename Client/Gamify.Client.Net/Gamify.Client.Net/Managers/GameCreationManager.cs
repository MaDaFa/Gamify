using Gamify.Client.Net.Client;
using Gamify.Client.Net.Services;
using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using System;

namespace Gamify.Client.Net.Managers
{
    public class GameCreationManager
    {
        private readonly IGameService<CreateGameRequestObject, GameInviteNotificationObject> createGameService;
        private readonly IGameService<GameAcceptedRequestObject, GameCreatedNotificationObject> acceptGameService;
        private readonly IGameService<GameRejectedRequestObject, GameRejectedNotificationObject> rejectGameService;

        public event EventHandler<GameNotificationEventArgs<GameInviteNotificationObject>> GameInviteNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<GameCreatedNotificationObject>> GameCreatedNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<GameRejectedNotificationObject>> GameRejectedNotificationReceived;

        public GameCreationManager(string playerName, IGameClientFactory clientFactory)
        {
            var gameClient = clientFactory.GetGameClient(playerName);

            this.createGameService = new GameService<CreateGameRequestObject, GameInviteNotificationObject>(GameRequestType.CreateGame, GameNotificationType.GameInvite, gameClient);
            this.acceptGameService = new GameService<GameAcceptedRequestObject, GameCreatedNotificationObject>(GameRequestType.GameAccepted, GameNotificationType.GameCreated, gameClient);
            this.rejectGameService = new GameService<GameRejectedRequestObject, GameRejectedNotificationObject>(GameRequestType.GameRejected, GameNotificationType.GameRejected, gameClient);

            this.createGameService.NotificationReceived += (sender, args) =>
            {
                this.NotifyGameInvite(args);
            };
            this.acceptGameService.NotificationReceived += (sender, args) =>
            {
                this.NotifyGameCreated(args);
            };
            this.rejectGameService.NotificationReceived += (sender, args) =>
            {
                this.NotifyGameRejected(args);
            };
        }

        public void CreateGame(CreateGameRequestObject createGameRequest)
        {
            this.createGameService.Send(createGameRequest);
        }

        public void AcceptGame(GameAcceptedRequestObject acceptGameGameRequest)
        {
            this.acceptGameService.Send(acceptGameGameRequest);
        }

        public void RejectGame(GameRejectedRequestObject rejectGameGameRequest)
        {
            this.rejectGameService.Send(rejectGameGameRequest);
        }

        private void NotifyGameInvite(GameNotificationEventArgs<GameInviteNotificationObject> args)
        {
            if (this.GameInviteNotificationReceived != null)
            {
                this.GameInviteNotificationReceived(this, args);
            }
        }

        private void NotifyGameCreated(GameNotificationEventArgs<GameCreatedNotificationObject> args)
        {
            if (this.GameCreatedNotificationReceived != null)
            {
                this.GameCreatedNotificationReceived(this, args);
            }
        }

        private void NotifyGameRejected(GameNotificationEventArgs<GameRejectedNotificationObject> args)
        {
            if (this.GameRejectedNotificationReceived != null)
            {
                this.GameRejectedNotificationReceived(this, args);
            }
        }
    }
}
