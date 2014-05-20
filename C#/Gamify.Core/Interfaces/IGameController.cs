using System.Collections.Generic;

namespace Gamify.Core.Interfaces
{
    public interface IGameController
    {
        IEnumerable<IGamePlayer> Players { get; }

        IEnumerable<IGameSession> Sessions { get; }

        void Connect(IGamePlayer player);

        IGameSession OpenSession(ISessionGamePlayerBase sessionPlayer1, ISessionGamePlayerBase sessionPlayer2 = null);

        IGameMoveResponse<U> HandleMove<T, U>(string playerName, string sessionId, IGameMove<T> move);

        void AbandonSession(string playerName, string sessionId);

        void Disconnect(string playerName);
    }
}
