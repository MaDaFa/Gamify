using Gamify.Sdk;

namespace Gamify.Service
{
    public interface IGamifyClientBase
    {
        string Id { get; }

        IGamePlayer Player { get; set; }
    }

    public interface IGamifyClient<T> : IGamifyClientBase
    {
        T Context { get; set; }
    }
}
