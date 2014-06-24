using Gamify.Sdk.Components;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.Setup
{
    public class GameBuilder<TMove, UResponse> : IGameBuilder<TMove, UResponse>
    {
        private readonly IPlayerService playerService;
        private readonly ISessionService sessionService;
        private readonly ISessionHistoryService<TMove, UResponse> sessionHistoryService;
        private readonly IMoveService<TMove, UResponse> moveService;
        private readonly INotificationService notificationService;
        private readonly ISerializer serializer;
        private readonly IGameService gameService;

        public GameBuilder(IPlayerService playerService, ISessionService sessionService,
            ISessionHistoryService<TMove, UResponse> sessionHistoryService, IMoveService<TMove, UResponse> moveService, 
            INotificationService notificationService, ISerializer serializer)
        {
            this.playerService = playerService;
            this.sessionService = sessionService;
            this.sessionHistoryService = sessionHistoryService;
            this.moveService = moveService;
            this.notificationService = notificationService;
            this.serializer = serializer;
            this.gameService = new GameService(this.serializer);
        }

        public void SetComponents(IGameDefinition<TMove, UResponse> gameDefinition)
        {
            this.gameService.RegisterComponent(new AbandonGameComponent(this.sessionService, this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new AcceptGameComponent(this.sessionService, this.notificationService, gameDefinition.GetSessionPlayerSetup(), this.serializer));
            this.gameService.RegisterComponent(new ActiveGamesComponent(this.sessionService, this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new PendingGamesComponent(this.sessionService, this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new FinishedGamesComponent(this.sessionService, this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new ConnectedPlayersComponent(this.playerService, this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new ConnectPlayerComponent(this.playerService, this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new CreateGameComponent(this.playerService, this.sessionService, this.notificationService, 
                gameDefinition.GetSessionPlayerFactory(), gameDefinition.GetSessionPlayerSetup(), gameDefinition.GetGameInviteDecorator(), this.serializer));
            this.gameService.RegisterComponent(new DisconnectPlayerComponent(this.playerService, this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new GameMoveComponent<TMove, UResponse>(this.moveService, this.sessionService, this.sessionHistoryService, 
                this.notificationService, gameDefinition.GetMoveFactory(), gameDefinition.GetMoveResultNotificationFactory(), this.serializer));
            this.gameService.RegisterComponent(new MessageComponent(this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new OpenGameComponent<TMove, UResponse>(this.sessionService, this.sessionHistoryService, 
                this.notificationService, gameDefinition.GetPlayerHistoryItemfactory(), this.serializer));
            this.gameService.RegisterComponent(new RejectGameComponent(this.sessionService, this.notificationService, this.serializer));

            foreach (var component in gameDefinition.GetCustomComponents())
            {
                this.gameService.RegisterComponent(component);
            }
        }

        public IGameService Build()
        {
            return this.gameService;
        }
    }
}
