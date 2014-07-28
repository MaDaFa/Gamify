using System.Collections.Generic;
using System.Linq;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace Gamify.Sdk.Contracts.ServerMessages
{
    public class FinishedGamesListServerMessage : IServerMessage
    {
        private readonly IList<GameObject> finishedGames;

        public string Message
        {
            get
            {
                return string.Format("There is a total of {0} finished games for Player {1}", this.FinishedGamesCount, this.PlayerName);
            }
        }

        public string PlayerName { get; set; }

        public IEnumerable<GameObject> FinishedGames
        {
            get
            {
                return this.finishedGames;
            }
        }

        public int FinishedGamesCount
        {
            get
            {
                return this.FinishedGames.Count();
            }
        }

        public FinishedGamesListServerMessage()
        {
            this.finishedGames = new List<GameObject>();
        }

        public void AddFinishedGame(GameObject finishedGame)
        {
            this.finishedGames.Add(finishedGame);
        }
    }
}
