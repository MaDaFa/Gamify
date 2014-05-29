using Gamify.Core;

namespace Gamify.Service.Interfaces
{
    public interface ISessionHistoryService<TMove, UResponse>
    {
        ISessionHistory<TMove, UResponse> GetBySessionPlayer(string sessionName, string playerName);

        bool Exist(string sessionName, string playerName);

        void Add(string sessionName, string playerName, ISessionHistoryItem<TMove, UResponse> historyItem);
    }
}
