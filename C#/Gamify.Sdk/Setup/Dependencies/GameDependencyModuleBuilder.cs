using Autofac;
using System;

namespace Gamify.Sdk.Setup.Dependencies
{
    public class GameDependencyModuleBuilder : IGameDependencyModuleBuilder
    {
        private readonly ContainerBuilder gameContainerBuilder;

        public GameDependencyModuleBuilder()
        {
            this.gameContainerBuilder = new ContainerBuilder();
        }

        public void SetDependency<T>()
        {
            this.gameContainerBuilder.RegisterType<T>();
        }

        public void SetDependency(Type type)
        {
            this.gameContainerBuilder.RegisterType(type);
        }

        public void SetDependency<T, U>() where U : T
        {
            this.gameContainerBuilder.RegisterType<U>().As<T>();
        }

        public void SetDependency(Type interfaceType, Type instanceType)
        {
            this.gameContainerBuilder.RegisterType(instanceType).As(interfaceType);
        }

        public void SetDependency<T>(Type instanceType)
        {
            this.gameContainerBuilder.RegisterType(instanceType).As<T>();
        }

        public void SetDependency<T>(T instance) where T : class
        {
            this.gameContainerBuilder.RegisterInstance(instance).As<T>();
        }

        public void SetDependency<T, U>(U instance) where U : class, T
        {
            this.gameContainerBuilder.RegisterInstance(instance).As<T>();
        }

        public void SetOpenGenericDependency(Type openGenericInterfaceType, Type openGenericType)
        {
            this.gameContainerBuilder.RegisterGeneric(openGenericType).As(openGenericInterfaceType);
        }

        public IGameDependencyModule Build()
        {
            var container = this.gameContainerBuilder.Build();
            var gameDependencyModule = new GameDependencyModule(container);

            return gameDependencyModule;
        }
    }
}
