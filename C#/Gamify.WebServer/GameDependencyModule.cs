using Autofac;
using Gamify.Sdk;
using Gamify.Sdk.Data;
using Gamify.Sdk.Data.Configuration;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;
using System;

namespace Gamify.WebServer
{
    public class GameDependencyModule : IGameDependencyModule
    {
        private readonly IGameDefinition gameDefinition;
        private IContainer gameContainer;

        public GameDependencyModule(IGameDefinition gameDefinition)
        {
            this.gameDefinition = gameDefinition;
        }

        public void Setup()
        {
            this.AutoConfigure();
        }

        public object Get(Type objectType)
        {
            return this.gameContainer.Resolve(objectType);
        }

        public T Get<T>()
        {
            return this.gameContainer.Resolve<T>();
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.gameContainer != null)
                {
                    this.gameContainer.Dispose();
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void AutoConfigure()
        {
            var containerBuilder = new ContainerBuilder();

            var gameConfiguration = GameDataSection.GetConfiguration();

            containerBuilder.RegisterInstance(gameConfiguration).As<IGameDataSection>();
            containerBuilder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>));
            containerBuilder.RegisterType<JsonSerializer>().As<ISerializer>();
            containerBuilder.RegisterType<NotificationService>().As<INotificationService>();
            containerBuilder.RegisterType<PlayerService>().As<IPlayerService>();
            containerBuilder.RegisterGeneric(typeof(SessionHistoryService<,>)).As(typeof(ISessionHistoryService<,>));
            containerBuilder.RegisterInstance(gameDefinition.GetSessionPlayerFactory()).As<ISessionPlayerFactory>();
            containerBuilder.RegisterType<SessionService>().As<ISessionService>();
            containerBuilder.RegisterType<MoveService>().As<IMoveService>();
            containerBuilder.RegisterGeneric(typeof(GameBuilder<,>)).As(typeof(IGameBuilder<,>));

            this.gameContainer = containerBuilder.Build();

            this.UpdateContainer((builder) =>
            {
                var sessionHistoryServiceInstance = this.gameContainer.Resolve(gameDefinition.GetSessionHistoryServiceType());

                builder.RegisterInstance(sessionHistoryServiceInstance).As<ISessionHistoryService>();
                builder.Update(this.gameContainer);
            });

            this.UpdateContainer((builder) =>
            {
                var gameBuilderInstance = this.gameContainer.Resolve(gameDefinition.GetGameBuilderType());

                builder.RegisterInstance(gameBuilderInstance).As<IGameBuilder>();
                builder.Update(this.gameContainer);
            });
        }

        private void UpdateContainer(Action<ContainerBuilder> updateAction)
        {
            var newContainerBuilder = new ContainerBuilder();

            updateAction.Invoke(newContainerBuilder);
        }
    }
}