using System.Collections.Generic;

namespace Gamify.Core
{
    public interface ISessionHistory<TMove, UResponse>
    {
        string SessionName { get; set; }

        string PlayerName { get; }

        IEnumerable<ISessionHistoryItem<TMove, UResponse>> Get();

        void Add(TMove move, UResponse response);
    }

    public interface ISessionHistoryItem<TMove, UResponse>
    {
        TMove Move { get; }

        UResponse Response { get; }
    }
}
