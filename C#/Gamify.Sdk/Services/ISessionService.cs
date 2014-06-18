using System.Collections.Generic;

namespace Gamify.Sdk.Services
{
    public interface ISessionService
    {
        IEnumerable<IGameSession> GetAll();

        IEnumerable<IGameSession> GetAllByPlayer(string playerName);

        IEnumerable<IGameSession> GetPendings(string playerName);

        IEnumerable<IGameSession> GetActives(string playerName);

        IEnumerable<IGameSession> GetFinished(string playerName);

        IGameSession GetByName(string sessionName);

        ///<exception cref="GameServiceException">GameServiceException</exception>
        IGameSession Create(SessionGamePlayer sessionPlayer1, SessionGamePlayer sessionPlayer2 = null);

        ///<exception cref="GameServiceException">GameServiceException</exception>
        void Start(IGameSession currentSession);

        ///<exception cref="GameServiceException">GameServiceException</exception>
        void Abandon(string sessionName);

        ///<exception cref="GameServiceException">GameServiceException</exception>
        void Finish(string sessionName);
    }
}
