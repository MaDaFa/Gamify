using System;

namespace Gamify.Sdk.Setup.Dependencies
{
    public interface IGameDependencyModuleBuilder
    {
        ///<exception cref="GameSetupException">GameSetupException</exception>
        void SetDependency<T>();

        ///<exception cref="GameSetupException">GameSetupException</exception>
        void SetDependency(Type type);

        ///<exception cref="GameSetupException">GameSetupException</exception>
        void SetDependency<T, U>() where U : T;

        ///<exception cref="GameSetupException">GameSetupException</exception>
        void SetDependency(Type interfaceType, Type instanceType);

        ///<exception cref="GameSetupException">GameSetupException</exception>
        void SetDependency<T>(Type instanceType);

        ///<exception cref="GameSetupException">GameSetupException</exception>
        void SetDependency<T>(T instance) where T : class;

        ///<exception cref="GameSetupException">GameSetupException</exception>
        void SetDependency<T, U>(U instance) where U : class, T;

        ///<exception cref="GameSetupException">GameSetupException</exception>
        void SetOpenGenericDependency(Type openGenericInterfaceType, Type openGenericType);

        ///<exception cref="GameSetupException">GameSetupException</exception>
        IGameDependencyModule Build();
    }
}
