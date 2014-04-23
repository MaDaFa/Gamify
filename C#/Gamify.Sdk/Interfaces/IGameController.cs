using System.Collections.Generic;

namespace Gamify.Sdk.Interfaces
{
    public interface IGameController
    {
		IEnumerable<IGamePlayer> Players { get; }

        IEnumerable<ISingleGameSession> GameSessions { get; }

		void ConnectPlayer (string playerName);

		void OpenSession (string playerName);

		void OpenSession (string playerName, string versusPlayerName);

		void CloseSession (string playerName);
    }
}
