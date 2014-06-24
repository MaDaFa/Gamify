using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.Components
{
    public class OpenGameComponent<TMove, UResponse> : GameComponent
    {
        private readonly ISessionService sessionService;
        private readonly ISessionHistoryService<TMove, UResponse> sessionHistoryService;
        private readonly IPlayerHistoryItemFactory<TMove, UResponse> playerHistoryItemFactory;
        private readonly ISerializer serializer;

        public OpenGameComponent(ISessionService sessionService, ISessionHistoryService<TMove, UResponse> sessionHistoryService,
            INotificationService notificationService, IPlayerHistoryItemFactory<TMove, UResponse> playerHistoryItemFactory, ISerializer serializer)
            : base(notificationService)
        {
            this.sessionService = sessionService;
            this.sessionHistoryService = sessionHistoryService;
            this.playerHistoryItemFactory = playerHistoryItemFactory;
            this.serializer = serializer;
        }

        public override bool CanHandleRequest(GameRequest request)
        {
            return request.Type == (int)GameRequestType.OpenGame;
        }

        public override bool CanHandleNotification(GameNotification notification)
        {
            return notification.Type == (int)GameNotificationType.SendGameInformation;
        }

        public override void HandleRequest(GameRequest request)
        {
            var openGameObject = this.serializer.Deserialize<OpenGameRequestObject>(request.SerializedRequestObject);
            var currentSession = this.sessionService.GetByName(openGameObject.SessionName);
            var gameInformationNotificationObject = this.GetNotificationObject(currentSession);

            this.notificationService.Send(GameNotificationType.SendGameInformation, gameInformationNotificationObject, openGameObject.PlayerName);
        }

        private GameInformationNotificationObject GetNotificationObject(IGameSession session)
        {
            var sessionPlayer1 = session.Player1;
            var sessionPlayer2 = session.Player2;
            var sessionPlayer1History = new PlayerHistoryObject(sessionPlayer1.Information.Name);
            var sessionPlayer2History = new PlayerHistoryObject(sessionPlayer2.Information.Name);

            var sessionPlayer1Moves = this.sessionHistoryService.GetBySessionPlayer(session.Name, sessionPlayer1.Information.Name).Get();

            foreach (var sessionPlayer1Move in sessionPlayer1Moves)
            {
                var historiItem = this.playerHistoryItemFactory.Create(sessionPlayer1Move.Response);

                sessionPlayer1History.AddMove(historiItem);
            }

            var sessionPlayer2Moves = this.sessionHistoryService.GetBySessionPlayer(session.Name, sessionPlayer2.Information.Name).Get();

            foreach (var sessionPlayer2Move in sessionPlayer2Moves)
            {
                var historiItem = this.playerHistoryItemFactory.Create(sessionPlayer2Move.Response);

                sessionPlayer2History.AddMove(historiItem);
            }

            var gameInformationNotificationObject = new GameInformationNotificationObject
            {
                SessionName = session.Name,
                Player1History = sessionPlayer1History,
                Player2History = sessionPlayer2History
            };

            return gameInformationNotificationObject;
        }
    }
}
