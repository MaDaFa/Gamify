using System;

namespace Gamify.WebServer
{
    public interface IGameDependencyModule : IDisposable
    {
        void Setup();

        object Get(Type objectType);

        T Get<T>();
    }
}
