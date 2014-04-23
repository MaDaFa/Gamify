using System.Collections.Generic;

namespace Gamify.Sdk.Interfaces
{
    public interface IGameController
    {
		IEnumerable<IGamePlayer> Players { get; }

        IEnumerable<ISingleGameSession> GameSessions { get; }

		void Connect (string playerName);

		void OpenSession (string playerName);

		void OpenSession (string playerName, string versusPlayerName);

		void Disconnect (string playerName);
    }
}
