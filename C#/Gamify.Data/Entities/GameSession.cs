using Gamify.Core;
using System;

namespace Gamify.Data.Entities
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
            else if (this.Player2.Information.UserName == playerName)
            {
                player = this.Player2;
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
