namespace Gamify.Sdk
{
    public interface IGameMoveResponse
    {
        bool IsWin { get; }

        object MoveResponseObject { get; }
    }

    public interface IGameMoveResponse<T> : IGameMoveResponse
    {
        new T MoveResponseObject { get; }
    }
}
