using System.Collections.Generic;
using System.Linq;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace Gamify.Sdk.Contracts.ServerMessages
{
    public class PendingGamesListServerMessage : IServerMessage
    {
        private readonly IList<GameObject> pendingGames;

        public string Message
        {
            get
            {
                return string.Format("There is a total of {0} pending games for Player {1}", this.PendingGamesCount, this.PlayerName);
            }
        }

        public string PlayerName { get; set; }

        public IEnumerable<GameObject> PendingGames
        {
            get
            {
                return this.pendingGames;
            }
        }

        public int PendingGamesCount
        {
            get
            {
                return this.PendingGames.Count();
            }
        }

        public PendingGamesListServerMessage()
        {
            this.pendingGames = new List<GameObject>();
        }

        public void AddPendingGame(GameObject pendingGame)
        {
            this.pendingGames.Add(pendingGame);
        }
    }
}
