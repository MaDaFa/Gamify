namespace Gamify.Sdk.Services
{
    public interface IMoveService<TMove, UResponse>
    {
        IGameMoveResponse<UResponse> Handle(string sessionName, string playerName, IGameMove<TMove> move);
    }
}
