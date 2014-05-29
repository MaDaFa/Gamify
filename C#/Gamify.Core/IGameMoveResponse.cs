namespace Gamify.Core
{
    public interface IGameMoveResponse<T>
    {
        T MoveResponseObject { get; }
    }
}
