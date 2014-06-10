using Gamify.Sdk.Services;

namespace Gamify.Sdk.Setup
{
    public interface IGameInitializer
    {
        IGameService Initialize();
    }
}
