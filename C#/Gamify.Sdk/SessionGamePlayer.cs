using Gamify.Sdk.Services;

namespace Gamify.Sdk
{
    public abstract class SessionGamePlayer<TMove, UResponse> : ISessionGamePlayer<TMove, UResponse>
    {
        private string sessionName;

        protected ISessionHistoryService<TMove, UResponse> sessionHistoryService;

        public bool IsReady { get; private set; }

        public IGamePlayer Information { get; set; }

        public string SessionName
        {
            get
            {
                return this.sessionName;
            }
            set
            {
                this.sessionName = value;
                this.IsReady = true;
            }
        }

        public bool PendingToMove { get; set; }

        protected SessionGamePlayer(ISessionHistoryService<TMove, UResponse> sessionHistoryService)
        {
            this.sessionHistoryService = sessionHistoryService;
        }

        public abstract ISessionHistory<TMove, UResponse> GetHistory();

        public abstract IGameMoveResponse<UResponse> ProcessMove(IGameMove<TMove> move);
    }
}
