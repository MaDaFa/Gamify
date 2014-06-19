using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.Setup
{
    public interface IGameInitializer
    {
        ///<exception cref="GameSetupException">GameSetupException</exception>
        IGameService Initialize<TMove, UResponse>(IGameDefinition<TMove, UResponse> gameDefinition);
    }
}
