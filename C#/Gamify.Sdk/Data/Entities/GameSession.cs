using System;

namespace Gamify.Sdk.Data.Entities
{
    public class GameSession : GameEntity, IGameSession
    {
        public SessionState State { get; set; }

        public string Name { get; set; }

        public SessionGamePlayer Player1 { get; set; }

        public string Player1Name { get; set; }

        public SessionGamePlayer Player2 { get; set; }

        public string Player2Name { get; set; }

        public GameSession(SessionGamePlayer player1, SessionGamePlayer player2)
        {
            this.Player1 = player1;
            this.Player1Name = player1.Information.Name;
            this.Player2 = player2;
            this.Player2Name = player2.Information.Name;
            this.Name = string.Concat(this.Player1Name, "-vs-", this.Player2Name);
            this.State = SessionState.Pending;

            this.Player1.SessionName = this.Name;
            this.Player1.PendingToMove = true;
            this.Player2.SessionName = this.Name;
        }

        public bool HasPlayer(string playerName)
        {
            return this.Player1Name == playerName || this.Player2Name == playerName;
        }

        public SessionGamePlayer GetPlayer(string playerName)
        {
            var player = default(SessionGamePlayer);

            this.ValidatePlayer(playerName);

            if (this.Player1Name == playerName)
            {
                player = this.Player1;
            }
            else
            {
                player = this.Player2;
            }

            return player;
        }

        public SessionGamePlayer GetVersusPlayer(string playerName)
        {
            var player = default(SessionGamePlayer);

            this.ValidatePlayer(playerName);

            if (this.Player1Name == playerName)
            {
                player = this.Player2;
            }
            else
            {
                player = this.Player1;
            }

            return player;
        }

        private void ValidatePlayer(string playerName)
        {
            if(!this.HasPlayer(playerName))
            {
                var message = string.Format("Player {0} is not part of this session", playerName);

                throw new ApplicationException(message);
            }
        }
    }
}
