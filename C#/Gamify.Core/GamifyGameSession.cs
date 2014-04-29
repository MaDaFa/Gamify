using Gamify.Sdk;
using System;

namespace Gamify.Service
{
    public class GamifyGameSession : IGameSession
    {
        public string Id { get; private set; }

        public bool IsReady
        {
            get
            {
                var player1Ready = this.Player1 != null && this.Player1.IsReady;
                var player2Ready = this.Player2 != null && this.Player2.IsReady;

                return player1Ready && player2Ready;
            }
        }

        public ISessionGamePlayerBase Player1 { get; private set; }

        public ISessionGamePlayerBase Player2 { get; private set; }

        public GamifyGameSession()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public GamifyGameSession(ISessionGamePlayerBase player1)
            : this()
        {
            this.AddPlayer(player1);
        }

        public GamifyGameSession(ISessionGamePlayerBase player1, ISessionGamePlayerBase player2)
            : this(player1)
        {
            this.AddPlayer(player2);
        }

        public void AddPlayer(ISessionGamePlayerBase player)
        {
            if (this.IsReady)
            {
                throw new ApplicationException("The game session is full");
            }

            if (this.Player1 == null)
            {
                this.Player1 = player;
            }
            else
            {
                this.Player2 = player;
            }
        }

        public ISessionGamePlayerBase GetPlayer(string playerName)
        {
            var player = default(ISessionGamePlayerBase);

            this.ValidatePlayer(playerName);

            if (this.Player1 != null && this.Player1.Information.UserName == playerName)
            {
                player = this.Player1;
            }
            else if (this.Player2 != null && this.Player2.Information.UserName == playerName)
            {
                player = this.Player2;
            }

            return player;
        }

        public void RemovePlayer(string playerName)
        {
            this.ValidatePlayer(playerName);

            if (this.Player1 != null && this.Player1.Information.UserName == playerName)
            {
                this.Player1 = null;
            }
            else if (this.Player2 != null && this.Player2.Information.UserName == playerName)
            {
                this.Player2 = null;
            }
        }

        public bool HasPlayer(string playerName)
        {
            return (this.Player1 != null && this.Player1.Information.UserName == playerName) || (this.Player2 != null && this.Player2.Information.UserName == playerName);
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
