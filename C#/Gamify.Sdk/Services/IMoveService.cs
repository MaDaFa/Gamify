namespace Gamify.Sdk.Services
{
    public interface IMoveService<TMove, UResponse>
    {
        ///<exception cref="GameServiceException">GameServiceException</exception>
        IGameMoveResponse<UResponse> Handle(string sessionName, string playerName, IGameMove<TMove> move);
    }
}
