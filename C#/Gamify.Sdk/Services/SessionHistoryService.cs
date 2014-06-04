using Gamify.Sdk.Data;
using Gamify.Sdk.Data.Entities;

namespace Gamify.Sdk.Services
{
    public class SessionHistoryService<TMove, UResponse> : ISessionHistoryService<TMove, UResponse>
    {
        private readonly IRepository<SessionHistory<TMove, UResponse>> sessionHistoryRepository;

        public SessionHistoryService(IRepository<SessionHistory<TMove, UResponse>> sessionHistoryRepository)
        {
            this.sessionHistoryRepository = sessionHistoryRepository;
        }

        public ISessionHistory<TMove, UResponse> GetBySessionPlayer(string sessionName, string playerName)
        {
            return this.sessionHistoryRepository.Get(h => h.SessionName == sessionName && h.PlayerName == playerName);
        }

        public bool Exist(string sessionName, string playerName)
        {
            return this.sessionHistoryRepository.Exist(h => h.SessionName == sessionName && h.PlayerName == playerName);
        }

        public void Add(string sessionName, string playerName, ISessionHistoryItem<TMove, UResponse> historyItem)
        {
            var existingHistory = this.sessionHistoryRepository.Get(h => h.SessionName == sessionName && h.PlayerName == playerName);

            if (existingHistory == null)
            {
                existingHistory = new SessionHistory<TMove, UResponse>(sessionName, playerName);
            }

            existingHistory.Add(historyItem.Move, historyItem.Response);

            if (existingHistory == null)
            {
                this.sessionHistoryRepository.Create(existingHistory);
            }
            else
            {
                this.sessionHistoryRepository.Update(existingHistory);
            }
        }
    }
}
