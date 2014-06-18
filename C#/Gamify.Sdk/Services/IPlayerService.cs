using System.Collections.Generic;

namespace Gamify.Sdk.Services
{
    public interface IPlayerService
    {
        IEnumerable<IGamePlayer> GetAll(string playerNameToExclude = null);

        IEnumerable<IGamePlayer> GetAllConnected(string playerNameToExclude = null);

        IGamePlayer GetByName(string playerName);

        IGamePlayer GetRandom(string playerNameToExclude = null);

        bool Exist(string playerName);

        ///<exception cref="GameServiceException">GameServiceException</exception>
        void Connect(string playerName, string name = null);

        ///<exception cref="GameServiceException">GameServiceException</exception>
        void Disconnect(string playerName);
    }
}
