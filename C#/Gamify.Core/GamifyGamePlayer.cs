using Gamify.Sdk;

namespace Gamify.Server
{
    public abstract class GamifyGamePlayer : IGamePlayerBase
    {
        public string Name { get; set; }

        public bool IsPlaying { get; set; }

        public abstract IGameMoveResponse<U> ProcessMove<T, U>(IGameMove<T> move);
    }
}
