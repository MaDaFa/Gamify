using System;

namespace Gamify.Sdk.Setup.Dependencies
{
    public interface IGameDependencyModuleBuilder
    {
        void SetDefaults();

        void Set<T>();

        void Set(Type type);

        void Set<T, U>() where U : T;

        void Set(Type interfaceType, Type instanceType);

        void Set<T>(Type instanceType);

        void Set<T>(T instance) where T : class;

        void Set<T, U>(U instance) where U : class, T;

        void SetOpenGeneric(Type openGenericInterfaceType, Type openGenericType);

        IGameDependencyModule Build();
    }
}
