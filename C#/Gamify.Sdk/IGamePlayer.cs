namespace Gamify.Sdk
{
    public interface IGamePlayer
    {
        string Name { get; }

        bool IsPlaying { get; }

        IGameMoveResponse<U> ProcessMove<T, U>(IGameMove<T> move);
    }
}
