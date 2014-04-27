namespace Gamify.Sdk
{
    public interface IGamePlayer<TMove, UResponse> : IGamePlayerBase
    {
        IGameMoveResponse<UResponse> ProcessMove(IGameMove<TMove> move);
    }

    public interface IGamePlayerBase
    {
        string Name { get; }

        bool IsPlaying { get; set; }
    }
}
