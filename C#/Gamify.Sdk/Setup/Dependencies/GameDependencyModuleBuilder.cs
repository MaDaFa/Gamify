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

        ///<exception cref="GameSetupException">GameSetupException</exception>
        public void SetDependency<T>()
        {
            this.SetDependency(() => 
            {
                this.gameContainerBuilder.RegisterType<T>();
            });
        }

        ///<exception cref="GameSetupException">GameSetupException</exception>
        public void SetDependency(Type type)
        {
            this.SetDependency(() =>
            {
                this.gameContainerBuilder.RegisterType(type);
            });
        }

        ///<exception cref="GameSetupException">GameSetupException</exception>
        public void SetDependency<T, U>() where U : T
        {
            this.SetDependency(() =>
            {
                this.gameContainerBuilder.RegisterType<U>().As<T>();
            });
        }

        ///<exception cref="GameSetupException">GameSetupException</exception>
        public void SetDependency(Type interfaceType, Type instanceType)
        {
            this.SetDependency(() =>
            {
                this.gameContainerBuilder.RegisterType(instanceType).As(interfaceType);
            });
        }

        ///<exception cref="GameSetupException">GameSetupException</exception>
        public void SetDependency<T>(Type instanceType)
        {
            this.SetDependency(() =>
            {
                this.gameContainerBuilder.RegisterType(instanceType).As<T>();
            });
        }

        ///<exception cref="GameSetupException">GameSetupException</exception>
        public void SetDependency<T>(T instance) where T : class
        {
            this.SetDependency(() =>
            {
                this.gameContainerBuilder.RegisterInstance(instance).As<T>();
            });
        }

        ///<exception cref="GameSetupException">GameSetupException</exception>
        public void SetDependency<T, U>(U instance) where U : class, T
        {
            this.SetDependency(() =>
            {
                this.gameContainerBuilder.RegisterInstance(instance).As<T>();
            });
        }

        ///<exception cref="GameSetupException">GameSetupException</exception>
        public void SetOpenGenericDependency(Type openGenericInterfaceType, Type openGenericType)
        {
            this.SetDependency(() =>
            {
                this.gameContainerBuilder.RegisterGeneric(openGenericType).As(openGenericInterfaceType);
            });
        }

        ///<exception cref="GameSetupException">GameSetupException</exception>
        public IGameDependencyModule Build()
        {
            var container = default(IContainer);

            try
            {
                container = this.gameContainerBuilder.Build();
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("An unexpected error occurred when building game dependencies. Details: {0}", ex.Message);

                throw new GameSetupException(errorMessage, ex);
            }

            var gameDependencyModule = new GameDependencyModule(container);

            return gameDependencyModule;
        }

        private void SetDependency(Action setDependencyAction)
        {
            try
            {
                setDependencyAction.Invoke();
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("An unexpected error occurred when setting up game dependencies. Details: {0}", ex.Message);

                throw new GameSetupException(errorMessage, ex);
            }
        }
    }
}
