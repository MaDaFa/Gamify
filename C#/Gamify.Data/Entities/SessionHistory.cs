using Gamify.Core;
using System.Collections.Generic;

namespace Gamify.Data.Entities
{
    public class SessionHistory<TMove, UResponse> : MongoEntity, ISessionHistory<TMove, UResponse>
    {
        public List<ISessionHistoryItem<TMove, UResponse>> Moves { get; set; }

        public string SessionName { get; set; }

        public string PlayerName { get; set; }

        public SessionHistory(string sessionName, string playerName)
        {
            this.SessionName = sessionName;
            this.PlayerName = playerName;
        }

        public IEnumerable<ISessionHistoryItem<TMove, UResponse>> Get()
        {
            return this.Moves;
        }

        public void Add(TMove move, UResponse response)
        {
            var item = new SessionGamePlayerHistoryItem<TMove, UResponse>
            {
                Move = move,
                Response = response
            };

            this.Moves.Add(item);
        }
    }

    public class SessionGamePlayerHistoryItem<TMove, UResponse> : ISessionHistoryItem<TMove, UResponse>
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
