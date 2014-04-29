using Gamify.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Service
{
    public abstract class GamifyGameController : IGameController
    {
        private readonly IList<IGamePlayer> players;
        private readonly IList<IGameSession> sessions;

        public IEnumerable<IGamePlayer> Players { get { return this.players; } }

        public IEnumerable<IGameSession> Sessions { get { return this.sessions; } }

        public GamifyGameController()
        {
            this.players = new List<IGamePlayer>();
            this.sessions = new List<IGameSession>();
        }

        protected abstract ISessionGamePlayerBase GetSessionPlayer(IGamePlayer player);

        public void Connect(IGamePlayer player)
        {
            this.ValidateNotExistingPlayer(player.UserName);

            this.players.Add(player);
        }

        public IGameSession OpenSession(ISessionGamePlayerBase sessionPlayer1, ISessionGamePlayerBase sessionPlayer2 = null)
        {
            if (sessionPlayer2 == null)
            {
                sessionPlayer2 = this.GetRandomSessionPlayer2(sessionPlayer1);
            }

            var newSession = new GamifyGameSession(sessionPlayer1, sessionPlayer2);

            this.sessions.Add(newSession);

            return newSession;
        }

        public IGameMoveResponse<U> HandleMove<T, U>(string playerName, string sessionId, IGameMove<T> move)
        {
            this.ValidatePlayerInSession(playerName, sessionId);

            var existingSession = this.Sessions.FirstOrDefault(s => s.Id == sessionId && s.HasPlayer(playerName));
            var playerToCall = existingSession.Player1.Information.UserName == playerName ? existingSession.Player2 : existingSession.Player1;

            return (playerToCall as ISessionGamePlayer<T,U>).ProcessMove(move);
        }

        public void AbandonSession(string playerName, string sessionId)
        {
            this.ValidatePlayerInSession(playerName, sessionId);

            var existingSession = this.Sessions.FirstOrDefault(s => s.Id == sessionId && s.HasPlayer(playerName));

            existingSession.RemovePlayer(playerName);
        }

        public void Disconnect(string playerName)
        {
            this.ValidateExistingPlayer(playerName);

            var existingSessions = this.Sessions.Where(s => s.HasPlayer(playerName));

            foreach (var existingSession in existingSessions)
            {
                existingSession.RemovePlayer(playerName);
            }

            var player = this.Players.FirstOrDefault(p => p.UserName == playerName);
            
            this.players.Remove(player);
        }

        private void ValidateNotExistingPlayer(string playerName)
        {
            if (this.Players.Any(p => p.UserName == playerName))
            {
                var errorMessage = string.Format("The player {0} is already connected", playerName);

                throw new ApplicationException(errorMessage);
            }
        }

        private void ValidateExistingPlayer(string playerName)
        {
            if (!this.Players.Any(p => p.UserName == playerName))
            {
                var errorMessage = string.Format("The player {0} is not connected", playerName);

                throw new ApplicationException(errorMessage);
            }
        }

        private void ValidateExistingSession(string sessionId)
        {
            if (!this.Sessions.Any(s => s.Id == sessionId))
            {
                var errorMessage = string.Format("The session {0} does not exists", sessionId);

                throw new ApplicationException(errorMessage);
            }
        }

        private void ValidatePlayerInSession(string playerName, string sessionId)
        {
            this.ValidateExistingPlayer(playerName);
            this.ValidateExistingSession(sessionId);

            var existingSession = this.Sessions.FirstOrDefault(s => s.Id == sessionId && s.HasPlayer(playerName));

            if (existingSession == null)
            {
                var errorMessage = string.Format("The player {0} is not part of the session {1}", playerName, sessionId);

                throw new ApplicationException(errorMessage);
            }
        }

        private ISessionGamePlayerBase GetRandomSessionPlayer2(ISessionGamePlayerBase sessionPlayer1)
        {
            var randomPlayer2 = this.Players
                .OrderBy(p => Guid.NewGuid())
                .FirstOrDefault(p => p.UserName != sessionPlayer1.Information.UserName);

            if (randomPlayer2 == null)
            {
                var errorMessage = "There are no users available to play";

                throw new ApplicationException(errorMessage);
            }

            return this.GetSessionPlayer(randomPlayer2);
        }
    }
}
