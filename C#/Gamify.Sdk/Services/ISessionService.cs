using System.Collections.Generic;

namespace Gamify.Sdk.Services
{
    public interface ISessionService
    {
        IEnumerable<IGameSession> GetAll();

        IEnumerable<IGameSession> GetAllByPlayer(string playerName);

        IEnumerable<IGameSession> GetPendings(string playerName);

        IEnumerable<IGameSession> GetActives(string playerName);

        IGameSession GetByName(string sessionName);

        IGameSession Create(SessionGamePlayer sessionPlayer1, SessionGamePlayer sessionPlayer2 = null);

        void Start(IGameSession currentSession);

        void Abandon(string sessionName);

        void Finish(string sessionName);
    }
}
