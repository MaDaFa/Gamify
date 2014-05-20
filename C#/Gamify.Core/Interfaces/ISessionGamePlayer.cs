namespace Gamify.Core.Interfaces
{
    public interface ISessionGamePlayerBase
    {
        IGamePlayer Information { get; set; }

        bool IsReady { get; set; }

        bool NeedsToMove { get; set; }
    }

    public interface ISessionGamePlayer<TMove, UResponse> : ISessionGamePlayerBase
    {
        ISessionGamePlayerHistory<TMove, UResponse> MovesHistory { get; }

        IGameMoveResponse<UResponse> ProcessMove(IGameMove<TMove> move);
    }
}
