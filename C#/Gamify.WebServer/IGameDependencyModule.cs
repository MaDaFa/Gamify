using Autofac;

namespace Gamify.WebServer
{
    public interface IGameDependencyModule
    {
        IContainer GetContainer();
    }
}
