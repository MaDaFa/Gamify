using System.Collections.Generic;
using ThinkUp.Sdk.Contracts.ServerMessages;

namespace Gamify.Sdk.Contracts.ServerMessages
{
    public class GameInformationReceivedServerMessage : IServerMessage
    {
        public string Message
        {
            get
            {
                return string.Format("Game information displayed for Player {0} and {1}", this.Player1History.PlayerName, this.Player2History.PlayerName);
            }
        }

        public string SessionName { get; set; }

        public PlayerHistoryObject Player1History { get; set; }

        public PlayerHistoryObject Player2History { get; set; }
    }

    public class PlayerHistoryObject
    {
        private readonly IList<IPlayerHistoryItem> moves;

        public string PlayerName { get; set; }

        public IEnumerable<IPlayerHistoryItem> Moves
        {
            get
            {
                return this.moves;
            }
        }

        public PlayerHistoryObject(string playerName)
        {
            this.moves = new List<IPlayerHistoryItem>();

            this.PlayerName = playerName;
        }

        public void AddMove(IPlayerHistoryItem move)
        {
            this.moves.Add(move);
        }
    }

    public interface IPlayerHistoryItem
    {
    }
}
