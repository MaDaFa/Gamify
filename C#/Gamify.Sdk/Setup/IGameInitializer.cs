using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.Setup
{
    public interface IGameInitializer
    {
        IGameService Initialize<TMove, UResponse>(IGameDefinition<TMove, UResponse> gameDefinition);
    }
}
