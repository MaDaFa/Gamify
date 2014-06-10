using Autofac;
using Gamify.Sdk.Data;
using Gamify.Sdk.Data.Configuration;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;
using System;

namespace Gamify.Sdk.Setup.Dependencies
{
    public class GameDependencyModuleBuilder : IGameDependencyModuleBuilder
    {
        private readonly IGameDefinition gameDefinition;
        private readonly ContainerBuilder gameContainerBuilder;

        public GameDependencyModuleBuilder(IGameDefinition gameDefinition)
        {
            this.gameDefinition = gameDefinition;
            this.gameContainerBuilder = new ContainerBuilder();
        }

        public void SetDefaults()
        {
            var gameConfiguration = GameDataSection.GetConfiguration();

            this.Set<IGameDataSection, GameDataSection>(gameConfiguration);
            this.SetOpenGeneric(typeof(IRepository<>), typeof(Repository<>));
            this.Set<ISerializer, JsonSerializer>();
            this.Set<INotificationService, NotificationService>();
            this.Set<IPlayerService, PlayerService>();
            this.SetOpenGeneric(typeof(ISessionHistoryService<,>), typeof(SessionHistoryService<,>));
            this.Set<ISessionHistoryService>(this.gameDefinition.GetSessionHistoryServiceType());
            this.Set<ISessionPlayerFactory>(gameDefinition.GetSessionPlayerFactory());
            this.Set<ISessionService, SessionService>();
            this.Set<IMoveService, MoveService>();
            this.Set<IGameBuilder, GameBuilder>();
        }

        public void Set<T>()
        {
            this.gameContainerBuilder.RegisterType<T>();
        }

        public void Set(Type type)
        {
            this.gameContainerBuilder.RegisterType(type);
        }

        public void Set<T, U>() where U : T
        {
            this.gameContainerBuilder.RegisterType<U>().As<T>();
        }

        public void Set(Type interfaceType, Type instanceType)
        {
            this.gameContainerBuilder.RegisterType(instanceType).As(interfaceType);
        }

        public void Set<T>(Type instanceType)
        {
            this.gameContainerBuilder.RegisterType(instanceType).As<T>();
        }

        public void Set<T>(T instance) where T : class
        {
            this.gameContainerBuilder.RegisterInstance(instance).As<T>();
        }

        public void Set<T, U>(U instance) where U : class, T
        {
            this.gameContainerBuilder.RegisterInstance(instance).As<T>();
        }

        public void SetOpenGeneric(Type openGenericInterfaceType, Type openGenericType)
        {
            this.gameContainerBuilder.RegisterGeneric(openGenericType).As(openGenericInterfaceType);
        }

        public IGameDependencyModule Build()
        {
            var container = this.gameContainerBuilder.Build();
            var gameDependencyModule = new GameDependencyModule(container, this.gameDefinition);

            return gameDependencyModule;
        }
    }
}
