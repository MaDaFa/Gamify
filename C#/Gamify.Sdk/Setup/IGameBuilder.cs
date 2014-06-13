using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.Setup
{
    public interface IGameBuilder<TMove, UResponse>
    {
        IGameService Build();

        void SetComponents(IGameDefinition<TMove, UResponse> gameDefinition);
    }
}
