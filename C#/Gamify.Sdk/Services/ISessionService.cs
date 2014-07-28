using Gamify.Sdk.Interfaces;
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

        ///<exception cref="ServiceException">ServiceException</exception>
        IGameSession Create(SessionGamePlayer sessionPlayer1, SessionGamePlayer sessionPlayer2 = null);

        ///<exception cref="ServiceException">ServiceException</exception>
        void Start(IGameSession currentSession);

        ///<exception cref="ServiceException">ServiceException</exception>
        void Abandon(string sessionName);

        ///<exception cref="ServiceException">ServiceException</exception>
        void Finish(string sessionName);
    }
}
