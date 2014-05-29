namespace Gamify.Core
{
    public interface ISessionGamePlayerBase
    {
        IGamePlayer Information { get; set; }

        bool IsReady { get; set; }

        bool PendingToMove { get; set; }
    }

    public interface ISessionGamePlayer<TMove, UResponse> : ISessionGamePlayerBase
    {
        ISessionHistory<TMove, UResponse> History { get; }

        IGameMoveResponse<UResponse> ProcessMove(IGameMove<TMove> move);
    }
}
