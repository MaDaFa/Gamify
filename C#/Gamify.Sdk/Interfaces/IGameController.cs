using System.Collections.Generic;

namespace Gamify.Sdk.Interfaces
{
    public interface IGameController
    {
		IEnumerable<IGamePlayer> Players { get; }

        IEnumerable<ISingleGameSession> GameSessions { get; }

		void AddPlayer (string playerName);

		void CreateSession (string playerName);

		void CreateSession (string playerName, string versusPlayerName);

		void CloseSession (string playerName);
    }
}
