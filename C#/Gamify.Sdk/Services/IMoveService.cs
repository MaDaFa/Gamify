namespace Gamify.Sdk.Services
{
    public interface IMoveService
    {
        IGameMoveResponse<U> Handle<T, U>(string sessionName, string playerName, IGameMove<T> move);
    }
}
