using Gamify.Core;
using System.Collections.Generic;

namespace Gamify.Service.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<IGameSession> GetAll();

        IEnumerable<IGameSession> GetAllByPlayer(string playerName);

        IGameSession GetByName(string sessionName);

        IGameSession Open(ISessionGamePlayerBase sessionPlayer1, ISessionGamePlayerBase sessionPlayer2 = null);

        void Abandon(string sessionName);

        void Finish(string sessionName);
    }
}
