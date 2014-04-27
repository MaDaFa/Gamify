using System.Collections.Generic;

namespace Gamify.Sdk
{
    public interface IGameController
    {
        IEnumerable<IGamePlayerBase> Players { get; }

        IEnumerable<IGameSession> GameSessions { get; }

        void Connect(IGamePlayerBase player);

        IGameSession OpenSession(string playerName, string versusPlayerName = null);

        IGameMoveResponse<U> HandleMove<T, U>(string playerName, string sessionId, IGameMove<T> move);

        void AbandonSession(string playerName, string sessionId);

        void Disconnect(string playerName);
    }
}
