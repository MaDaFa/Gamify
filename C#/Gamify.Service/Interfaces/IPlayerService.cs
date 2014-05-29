using Gamify.Core;
using System.Collections.Generic;

namespace Gamify.Service.Interfaces
{
    public interface IPlayerService
    {
        IEnumerable<IGamePlayer> GetAll(string playerNameToExclude = null);

        IGamePlayer GetByName(string playerName);

        IGamePlayer GetRandom(string playerNameToExclude = null);

        bool Exist(string playerName);

        void Create(string userName, string name);
    }
}
