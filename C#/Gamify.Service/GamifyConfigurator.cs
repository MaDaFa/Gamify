using Gamify.Core.Interfaces;
using Gamify.Service.Components;

namespace Gamify.Service
{
    public class GamifyConfigurator : IGameConfigurator
    {
        private readonly INotificationService notificationService;
        private readonly IGameController gameController;

        public GamifyConfigurator(INotificationService notificationService, IGameController gameController)
        {
            this.notificationService = notificationService;
            this.gameController = gameController;
        }

        public void Configure(IGameServiceSetup gameServiceSetup)
        {
            gameServiceSetup.RegisterComponent(new AbandonGameComponent(this.notificationService));
            gameServiceSetup.RegisterComponent(new ActiveGamesComponent(this.notificationService, this.gameController));
            gameServiceSetup.RegisterComponent(new ConnectedPlayersComponent(this.notificationService, this.gameController));
            gameServiceSetup.RegisterComponent(new DisconnectPlayerComponent(this.notificationService, this.gameController));
            gameServiceSetup.RegisterComponent(new MessageComponent(this.notificationService));
            gameServiceSetup.RegisterComponent(new RejectGameComponent(this.notificationService, this.gameController));
        }
    }
}
