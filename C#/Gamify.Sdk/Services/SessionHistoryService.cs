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

        ///<exception cref="GameServiceException">GameServiceException</exception>
        public void Add(string sessionName, string playerName, ISessionHistoryItem<TMove, UResponse> historyItem)
        {
            var historyExists = true;
            var existingHistory = this.sessionHistoryRepository.Get(h => h.SessionName == sessionName && h.PlayerName == playerName);

            if (existingHistory == null)
            {
                historyExists = false;
                existingHistory = new SessionHistory<TMove, UResponse>(sessionName, playerName);
            }

            existingHistory.Add(historyItem.Move, historyItem.Response);

            try
            {
                if (historyExists)
                {
                    this.sessionHistoryRepository.Update(existingHistory);
                }
                else
                {
                    this.sessionHistoryRepository.Create(existingHistory);
                }
            }
            catch (GameDataException gameDataEx)
            {
                var actionKeyword = historyExists ? "updating" : "creating";
                var errorMessage = string.Format("An error occured when {0} the history for player {1} and session {2}", actionKeyword, playerName, sessionName);

                throw new GameServiceException(errorMessage, gameDataEx);
            }
        }
    }
}
