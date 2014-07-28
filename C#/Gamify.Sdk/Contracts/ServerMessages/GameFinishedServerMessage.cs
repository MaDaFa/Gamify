using ThinkUp.Sdk.Contracts.ServerMessages;

namespace Gamify.Sdk.Contracts.ServerMessages
{
    public class GameFinishedServerMessage : IServerMessage
    {
        public string Message
        {
            get
            {
                return string.Format("{0} won the game against {1}. The game has finished", this.WinnerPlayerName, this.LooserPlayerName);
            }
        }

        public string SessionName { get; set; }

        public string WinnerPlayerName { get; set; }

        public string LooserPlayerName { get; set; }
    }
}
