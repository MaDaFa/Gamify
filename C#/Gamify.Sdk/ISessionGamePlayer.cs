namespace Gamify.Sdk
{
    public interface ISessionGamePlayerBase
    {
        IGamePlayer Information { get; set; }

        string SessionName { get; set; }

        bool IsReady { get; set; }

        bool PendingToMove { get; set; }
    }

    public interface ISessionGamePlayer<TMove, UResponse> : ISessionGamePlayerBase
    {
        ISessionHistory<TMove, UResponse> GetHistory();

        IGameMoveResponse<UResponse> ProcessMove(IGameMove<TMove> move);
    }
}
