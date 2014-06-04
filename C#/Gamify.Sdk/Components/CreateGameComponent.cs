using Gamify.Contracts.Requests;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;

namespace Gamify.Sdk.Components
{
    public class CreateGameComponent : IGameComponent
    {
        private readonly IPlayerService playerService;
        private readonly ISessionService sessionService;
        private readonly ISessionHistoryService sessionHistoryService;
        private readonly ISessionPlayerFactory sessionPlayerFactory;
        private readonly ISessionPlayerSetup playerSetup;
        private readonly IGameInviteDecorator gameInviteDecorator;
        private readonly ISerializer serializer;

        public INotificationService NotificationService { get; private set; }

        public CreateGameComponent(IPlayerService playerService, ISessionService sessionService, 
            ISessionHistoryService sessionHistoryService, INotificationService notificationService, 
            ISessionPlayerFactory sessionPlayerFactory, ISessionPlayerSetup playerSetup, 
            IGameInviteDecorator gameInviteDecorator, ISerializer serializer)
        {
            this.playerService = playerService;
            this.sessionService = sessionService;
            this.sessionHistoryService = sessionHistoryService;
            this.sessionPlayerFactory = sessionPlayerFactory;
            this.playerSetup = playerSetup;
            this.gameInviteDecorator = gameInviteDecorator;
            this.serializer = serializer;

            this.NotificationService = notificationService;
        }

        public bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.CreateGame;
        }

        public void HandleRequest(GameRequest request)
        {
            var createGameObject = this.serializer.Deserialize<CreateGameRequestObject>(request.SerializedRequestObject);
            var connectedPlayer1 = this.playerService.GetByName(createGameObject.PlayerName);
            var sessionPlayer1 = this.sessionPlayerFactory.Create(connectedPlayer1, sessionHistoryService);
            var connectedPlayer2 = this.playerService.GetByName(createGameObject.InvitedPlayerName);
            var sessionPlayer2 = this.sessionPlayerFactory.Create(connectedPlayer2, sessionHistoryService);

            this.playerSetup.GetPlayerReady(createGameObject, sessionPlayer1);

            var newSession = this.sessionService.Open(sessionPlayer1, sessionPlayer2);

            this.SendGameInviteNotification(newSession);
        }

        private void SendGameInviteNotification(IGameSession newSession)
        {
            var gameInviteNotificationObject = new GameInviteNotificationObject
            {
                SessionName = newSession.Name,
                Player1Name = newSession.Player1.Information.UserName
            };

            this.gameInviteDecorator.Decorate(gameInviteNotificationObject, newSession);

            this.NotificationService.Send(GameNotificationType.GameInvite, gameInviteNotificationObject, newSession.Player2.Information.UserName);
        }
    }
}
