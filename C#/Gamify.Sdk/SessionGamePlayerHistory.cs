using System.Collections.Generic;

namespace Gamify.Sdk
{
    public class SessionGamePlayerHistory<TMove, UResponse> : ISessionGamePlayerHistory<TMove, UResponse>
    {
        private readonly IList<ISessionGamePlayerHistoryItem<TMove, UResponse>> moves;

        public IEnumerable<ISessionGamePlayerHistoryItem<TMove, UResponse>> Moves
        {
            get { return this.moves; }
        }

        public SessionGamePlayerHistory()
        {
            this.moves = new List<ISessionGamePlayerHistoryItem<TMove, UResponse>>();
        }

        public void Add(TMove move, UResponse response)
        {
            var item = new SessionGamePlayerHistoryItem<TMove, UResponse>
            {
                Move = move,
                Response = response
            };

            this.moves.Add(item);
        }
    }

    public class SessionGamePlayerHistoryItem<TMove, UResponse> : ISessionGamePlayerHistoryItem<TMove, UResponse>
    {
        public TMove Move { get; set; }

        public UResponse Response { get; set; }

        public SessionGamePlayerHistoryItem(TMove move, UResponse response)
        {
            this.Move = move;
            this.Response = response;
        }

        public SessionGamePlayerHistoryItem()
        {
        }
    }
}
