using Gamify.Service.Components;
using Gamify.Service.Interfaces;

namespace Gamify.Service
{
    public class GamifyConfigurator : IGameConfigurator
    {
        private readonly IPlayerService playerService;
        private readonly ISessionService sessionService;
        private readonly INotificationService notificationService;

        public GamifyConfigurator(IPlayerService playerService, ISessionService sessionService, INotificationService notificationService)
        {
            this.playerService = playerService;
            this.sessionService = sessionService;
            this.notificationService = notificationService;
        }

        public void Configure(IGameServiceSetup gameServiceSetup)
        {
            gameServiceSetup.RegisterComponent(new ConnectPlayerComponent(this.playerService, this.notificationService));
            gameServiceSetup.RegisterComponent(new AbandonGameComponent(this.sessionService, this.notificationService));
            gameServiceSetup.RegisterComponent(new ActiveGamesComponent(this.sessionService, this.notificationService));
            gameServiceSetup.RegisterComponent(new ConnectedPlayersComponent(this.playerService, this.notificationService));
            gameServiceSetup.RegisterComponent(new DisconnectPlayerComponent(this.playerService, this.notificationService));
            gameServiceSetup.RegisterComponent(new MessageComponent(this.notificationService));
            gameServiceSetup.RegisterComponent(new RejectGameComponent(this.sessionService, this.notificationService));
        }
    }
}
