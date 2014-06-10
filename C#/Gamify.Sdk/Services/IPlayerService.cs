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

        void Connect(string playerName, string name = null);

        void Disconnect(string playerName);
    }
}
