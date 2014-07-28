using Gamify.Sdk.Interfaces;

namespace Gamify.Sdk.Services
{
    public interface IMoveService<TMove, UResponse>
    {
        ///<exception cref="ServiceException">ServiceException</exception>
        IGameMoveResponse<UResponse> Handle(string sessionName, string playerName, IGameMove<TMove> move);
    }
}
