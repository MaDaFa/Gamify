using Gamify.Sdk.Components;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.Setup
{
    public class GameBuilder : IGameBuilder
    {
        private readonly IPlayerService playerService;
        private readonly ISessionService sessionService;
        private readonly ISessionHistoryService sessionHistoryService;
        private readonly IMoveService moveService;
        private readonly INotificationService notificationService;
        private readonly IGameService gameService;
        private readonly ISerializer serializer;

        public GameBuilder(IPlayerService playerService, ISessionService sessionService, 
            ISessionHistoryService sessionHistoryService,
            IMoveService moveService, INotificationService notificationService, ISerializer serializer)
        {
            this.playerService = playerService;
            this.sessionService = sessionService;
            this.sessionHistoryService = sessionHistoryService;
            this.moveService = moveService;
            this.notificationService = notificationService;
            this.serializer = serializer;
            this.gameService = new GameService(serializer);
        }

        public void SetComponents(IGameDefinition gameDefinition)
        {
            this.gameService.RegisterComponent(new AbandonGameComponent(this.sessionService, this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new AcceptGameComponent(this.sessionService, this.notificationService, gameDefinition.GetSessionPlayerSetup(), this.serializer));
            this.gameService.RegisterComponent(new ActiveGamesComponent(this.sessionService, this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new ConnectedPlayersComponent(this.playerService, this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new ConnectPlayerComponent(this.playerService, this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new CreateGameComponent(this.playerService, this.sessionService, this.sessionHistoryService,
                this.notificationService, gameDefinition.GetSessionPlayerFactory(),
                gameDefinition.GetSessionPlayerSetup(), gameDefinition.GetGameInviteDecorator(), this.serializer));
            this.gameService.RegisterComponent(new DisconnectPlayerComponent(this.playerService, this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new GameMoveComponent(this.moveService, this.sessionService, this.notificationService,
                gameDefinition.GetMoveHandler(), gameDefinition.GetMoveResultNotificationFactory(), this.serializer));
            this.gameService.RegisterComponent(new MessageComponent(this.notificationService, this.serializer));
            this.gameService.RegisterComponent(new OpenGameComponent(this.sessionService, this.notificationService, gameDefinition.GetGameInformationNotificationFactory(), this.serializer));

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
