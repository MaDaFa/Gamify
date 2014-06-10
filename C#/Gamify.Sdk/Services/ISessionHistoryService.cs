namespace Gamify.Sdk.Services
{
    public interface ISessionHistoryService
    {
        bool Exist(string sessionName, string playerName);
    }

    public interface ISessionHistoryService<TMove, UResponse> : ISessionHistoryService
    {
        void Add(string sessionName, string playerName, ISessionHistoryItem<TMove, UResponse> historyItem);

        ISessionHistory<TMove, UResponse> GetBySessionPlayer(string sessionName, string playerName);
    }
}
