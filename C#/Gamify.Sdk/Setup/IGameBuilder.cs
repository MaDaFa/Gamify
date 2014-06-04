using Gamify.Sdk.Services;

namespace Gamify.Sdk.Setup
{
    public interface IGameBuilder
    {
        IGameService Build();
    }

    public interface IGameBuilder<TMove, UResponse> : IGameBuilder
    {
        void SetComponents(IGameDefinition<TMove, UResponse> gameDefinition);
    }
}
