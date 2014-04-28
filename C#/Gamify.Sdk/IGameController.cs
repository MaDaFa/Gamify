using System.Collections.Generic;

namespace Gamify.Sdk
{
    public interface IGameController
    {
        IEnumerable<IGamePlayer> Players { get; }

        IEnumerable<IGameSession> Sessions { get; }

        void Connect(IGamePlayer player);

        IGameSession OpenSession(string playerName, string versusPlayerName = null);

        IGameMoveResponse<U> HandleMove<T, U>(string playerName, string sessionId, IGameMove<T> move);

        void AbandonSession(string playerName, string sessionId);

        void Disconnect(string playerName);
    }
}
