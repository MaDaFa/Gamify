using System;

namespace Gamify.Sdk.Data.Entities
{
    public class GameSession : MongoEntity, IGameSession
    {
        public SessionState State { get; set; }

        public string Name { get; set; }

        public ISessionGamePlayerBase Player1 { get; set; }

        public ISessionGamePlayerBase Player2 { get; set; }

        public GameSession(ISessionGamePlayerBase player1, ISessionGamePlayerBase player2)
        {
            this.Player1 = player1;
            this.Player2 = player2;
            this.Name = string.Concat(this.Player1.Information.Name, "-vs-", this.Player2.Information.Name);
            this.State = SessionState.Active;

            this.Player1.SessionName = this.Name;
            this.Player2.SessionName = this.Name;
        }

        public bool HasPlayer(string playerName)
        {
            return this.Player1.Information.UserName == playerName || this.Player2.Information.UserName == playerName;
        }

        public ISessionGamePlayerBase GetPlayer(string playerName)
        {
            var player = default(ISessionGamePlayerBase);

            this.ValidatePlayer(playerName);

            if (this.Player1.Information.UserName == playerName)
            {
                player = this.Player1;
            }
            else
            {
                player = this.Player2;
            }

            return player;
        }

        public ISessionGamePlayerBase GetVersusPlayer(string playerName)
        {
            var player = default(ISessionGamePlayerBase);

            this.ValidatePlayer(playerName);

            if (this.Player1.Information.UserName == playerName)
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
