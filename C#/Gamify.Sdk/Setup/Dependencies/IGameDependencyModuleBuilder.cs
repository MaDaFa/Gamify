using System;

namespace Gamify.Sdk.Setup.Dependencies
{
    public interface IGameDependencyModuleBuilder
    {
        void SetDependency<T>();

        void SetDependency(Type type);

        void SetDependency<T, U>() where U : T;

        void SetDependency(Type interfaceType, Type instanceType);

        void SetDependency<T>(Type instanceType);

        void SetDependency<T>(T instance) where T : class;

        void SetDependency<T, U>(U instance) where U : class, T;

        void SetOpenGenericDependency(Type openGenericInterfaceType, Type openGenericType);

        IGameDependencyModule Build();
    }
}
