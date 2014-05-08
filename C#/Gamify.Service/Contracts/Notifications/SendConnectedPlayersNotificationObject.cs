using System.Collections.Generic;

namespace Gamify.Service.Contracts.Notifications
{
    public class SendConnectedPlayersNotificationObject : INotificationObject
    {
        public string Message
        {
            get
            {
                return string.Format("There is a total of {0} players connected", this.ConectedPlayersCount);
            }
        }

        public string PlayerName { get; set; }

        public IEnumerable<string> ConnectedPlayerNames { get; set; }

        public int ConectedPlayersCount { get; set; }
    }
}
