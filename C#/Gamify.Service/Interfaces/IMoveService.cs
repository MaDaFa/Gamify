using Gamify.Core;

namespace Gamify.Service.Interfaces
{
    public interface IMoveService
    {
        IGameMoveResponse<U> Handle<T, U>(string sessionName, string playerName, IGameMove<T> move);
    }
}
