using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;

namespace Gamify.Sdk.Setup
{
    public interface IGameBuilder
    {
        IGameService Build();

        void SetComponents(IGameDefinition gameDefinition);
    }
}
