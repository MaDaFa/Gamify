using Gamify.Sdk;

namespace Gamify.Service
{
    public interface IGamifyClientBase
    {
        IGamePlayer Player { get; set; }
    }

    public interface IGamifyClient<T> : IGamifyClientBase
    {
        T Context { get; set; }
    }
}
