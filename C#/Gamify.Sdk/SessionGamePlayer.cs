using Gamify.Sdk.Services;

namespace Gamify.Sdk
{
    public abstract class SessionGamePlayer<TMove, UResponse> : ISessionGamePlayer<TMove, UResponse>
    {
        protected ISessionHistoryService<TMove, UResponse> sessionHistoryService;

        public IGamePlayer Information { get; set; }

        public string SessionName { get; set; }

        public bool IsReady { get; set; }

        public bool PendingToMove { get; set; }

        protected SessionGamePlayer(ISessionHistoryService<TMove, UResponse> sessionHistoryService)
        {
            this.sessionHistoryService = sessionHistoryService;
        }

        public abstract ISessionHistory<TMove, UResponse> GetHistory();

        public abstract IGameMoveResponse<UResponse> ProcessMove(IGameMove<TMove> move);
    }
}
