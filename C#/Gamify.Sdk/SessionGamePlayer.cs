using ThinkUp.Sdk.Interfaces;

namespace Gamify.Sdk
{
    public class SessionGamePlayer
    {
        private string sessionName;

        public bool IsReady { get; private set; }

        public IUser Information { get; set; }

        public string SessionName
        {
            get
            {
                return this.sessionName;
            }
            set
            {
                this.sessionName = value;
                this.IsReady = true;
            }
        }

        public bool PendingToMove { get; set; }
    }
}
