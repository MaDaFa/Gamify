using ThinkUp.Sdk.Contracts.ServerMessages;

namespace Gamify.Sdk.Contracts.ServerMessages
{
    public class GameCreatedServerMessage : IServerMessage
    {
        public string Message
        {
            get
            {
                return string.Format("A new game between {0} and {1} has been created", this.Player1Name, this.Player2Name);
            }
        }

        public string SessionName { get; set; }

        public string Player1Name { get; set; }

        public string Player2Name { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
