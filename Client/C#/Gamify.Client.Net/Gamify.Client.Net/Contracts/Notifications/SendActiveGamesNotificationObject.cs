using System.Collections.Generic;
using System.Linq;

namespace Gamify.Client.Net.Contracts.Notifications
{
    public class SendActiveGamesNotificationObject : INotificationObject
    {
        private readonly IList<GameObject> activeGames;

        public string Message
        {
            get
            {
                return string.Format("There is a total of {0} active games for Player {1}", this.ActiveGamesCount, this.PlayerName);
            }
        }

        public string PlayerName { get; set; }

        public IEnumerable<GameObject> ActiveGames
        {
            get
            {
                return this.activeGames;
            }
        }

        public int ActiveGamesCount
        {
            get
            {
                return this.ActiveGames.Count();
            }
        }

        public SendActiveGamesNotificationObject()
        {
            this.activeGames = new List<GameObject>();
        }

        public void AddActiveGame(GameObject activeGame)
        {
            this.activeGames.Add(activeGame);
        }
    }
}
