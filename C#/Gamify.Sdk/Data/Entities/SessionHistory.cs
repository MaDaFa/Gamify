using System.Collections.Generic;

namespace Gamify.Sdk.Data.Entities
{
    public class SessionHistory<TMove, UResponse> : GameEntity, ISessionHistory<TMove, UResponse>
    {
        public List<SessionHistoryItem<TMove, UResponse>> Moves { get; set; }

        public string SessionName { get; set; }

        public string PlayerName { get; set; }

        public SessionHistory(string sessionName, string playerName)
        {
            this.Moves = new List<SessionHistoryItem<TMove, UResponse>>();

            this.SessionName = sessionName;
            this.PlayerName = playerName;
        }

        public IEnumerable<ISessionHistoryItem<TMove, UResponse>> Get()
        {
            return this.Moves;
        }

        public void Add(TMove move, UResponse response)
        {
            var item = new SessionHistoryItem<TMove, UResponse>
            {
                Move = move,
                Response = response
            };

            this.Moves.Add(item);
        }
    }

    public class SessionHistoryItem<TMove, UResponse> : ISessionHistoryItem<TMove, UResponse>
    {
        public TMove Move { get; set; }

        public UResponse Response { get; set; }

        public SessionHistoryItem(TMove move, UResponse response)
        {
            this.Move = move;
            this.Response = response;
        }

        public SessionHistoryItem()
        {
        }
    }
}
