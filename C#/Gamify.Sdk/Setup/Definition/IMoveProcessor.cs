namespace Gamify.Sdk.Setup.Definition
{
    public interface IMoveProcessor<TMove, UResponse>
    {
        IGameMoveResponse<UResponse> Process(SessionGamePlayer sessionGamePlayer, IGameMove<TMove> move);
    }
}
