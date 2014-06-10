using System;

namespace Gamify.Sdk.Setup.Dependencies
{
    public interface IGameDependencyModule : IDisposable
    {
        object Get(Type objectType);

        T Get<T>();
    }
}
