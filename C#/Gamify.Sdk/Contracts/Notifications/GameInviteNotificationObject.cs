using System;

namespace Gamify.Sdk.Contracts.Notifications
{
    public class GameInviteNotificationObject : INotificationObject
    {
        public string Message
        {
            get
            {
                return string.Format("The player {0} has sent an invitation to join Game {1}", this.Player1Name, this.SessionName);
            }
        }

        public string SessionName { get; set; }

        public string Player1Name { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
