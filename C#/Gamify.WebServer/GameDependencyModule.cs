using Autofac;
using Gamify.Sdk;
using Gamify.Sdk.Data;
using Gamify.Sdk.Data.Configuration;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;

namespace Gamify.WebServer
{
    public class GameDependencyModule : IGameDependencyModule
    {
        private IContainer gameContainer;

        public GameDependencyModule(IGameDefinition gameDefinition)
        {
            this.RegisterInstances(gameDefinition);
        }

        public IContainer GetContainer()
        {
            return this.gameContainer;
        }

        private void RegisterInstances(IGameDefinition gameDefinition)
        {
            var containerBuilder = new ContainerBuilder();

            var gameConfiguration = GameDataSection.GetConfiguration();

            containerBuilder.RegisterInstance(gameConfiguration).As<IGameDataSection>();
            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            containerBuilder.RegisterType<JsonSerializer>().As<ISerializer>();
            containerBuilder.RegisterType<NotificationService>().As<INotificationService>();
            containerBuilder.RegisterType<PlayerService>().As<IPlayerService>();
            containerBuilder.RegisterGeneric(typeof(SessionHistoryService<,>)).As(typeof(ISessionHistoryService<,>), typeof(ISessionHistoryService));
            containerBuilder.RegisterInstance(gameDefinition.GetSessionPlayerFactory()).As<ISessionPlayerFactory>();
            containerBuilder.RegisterType<SessionService>().As<ISessionService>();
            containerBuilder.RegisterType<MoveService>().As<IMoveService>();
            containerBuilder.RegisterGeneric(typeof(GameBuilder<,>)).As(typeof(IGameBuilder<,>), typeof(IGameBuilder));

            this.gameContainer = containerBuilder.Build();
        }
    }
}