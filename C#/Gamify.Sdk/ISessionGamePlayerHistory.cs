using System.Collections.Generic;

namespace Gamify.Sdk
{
    public interface ISessionGamePlayerHistory<TMove, UResponse>
    {
        IEnumerable<ISessionGamePlayerHistoryItem<TMove, UResponse>> Moves { get; }

        void Add(TMove move, UResponse response);
    }

    public interface ISessionGamePlayerHistoryItem<TMove, UResponse>
    {
        TMove Move { get; }

        UResponse Response { get; }
    }
}
