using System;

namespace Gamify.Sdk.Setup.Dependencies
{
    public interface IGameDependencyModule : IDisposable
    {
        ///<exception cref="GameSetupException">GameSetupException</exception>
        object Get(Type objectType);

        ///<exception cref="GameSetupException">GameSetupException</exception>
        T Get<T>();
    }
}
