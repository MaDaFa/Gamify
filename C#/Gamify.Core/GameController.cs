using Gamify.Core.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Core
{
    public abstract class GameController : IGameController
    {
        private static readonly ConcurrentDictionary<string, IGamePlayer> players;
        private static readonly ConcurrentDictionary<string, IGameSession> sessions;

        public IEnumerable<IGamePlayer> Players { get { return players.Values; } }

        public IEnumerable<IGameSession> Sessions { get { return sessions.Values; } }

        static GameController()
        {
            players = new ConcurrentDictionary<string, IGamePlayer>();
            sessions = new ConcurrentDictionary<string, IGameSession>();
        }

        protected GameController()
        {
        }

        protected abstract ISessionGamePlayerBase GetSessionPlayer(IGamePlayer player);

        public void Connect(IGamePlayer player)
        {
            this.ValidateNotExistingPlayer(player.UserName);

            players.TryAdd(player.UserName, player);
        }

        public IGameSession OpenSession(ISessionGamePlayerBase sessionPlayer1, ISessionGamePlayerBase sessionPlayer2 = null)
        {
            if (sessionPlayer2 == null)
            {
                sessionPlayer2 = this.GetRandomSessionPlayer2(sessionPlayer1);
            }

            var newSession = new GameSession(sessionPlayer1, sessionPlayer2);

            sessions.TryAdd(newSession.Id, newSession);

            return newSession;
        }

        public IGameMoveResponse<U> HandleMove<T, U>(string playerName, string sessionId, IGameMove<T> move)
        {
            this.ValidatePlayerInSession(playerName, sessionId);

            var existingSession = sessions
                .Where(s => s.Key == sessionId && s.Value.HasPlayer(playerName))
                .Select(s => s.Value)
                .First();
            var playerToCall = existingSession.Player1.Information.UserName == playerName ? existingSession.Player2 : existingSession.Player1;

            return (playerToCall as ISessionGamePlayer<T,U>).ProcessMove(move);
        }

        public void AbandonSession(string playerName, string sessionId)
        {
            this.ValidatePlayerInSession(playerName, sessionId);

            var existingSession = sessions
                .Where(s => s.Key == sessionId && s.Value.HasPlayer(playerName))
                .Select(s => s.Value)
                .First();

            existingSession.RemovePlayer(playerName);
        }

        public void Disconnect(string playerName)
        {
            this.ValidateExistingPlayer(playerName);

            var existingSessions = sessions
                .Where(s => s.Value.HasPlayer(playerName))
                .Select(s => s.Value);

            foreach (var existingSession in existingSessions)
            {
                existingSession.RemovePlayer(playerName);
            }

            var removedPlayer = default(IGamePlayer);

            players.TryRemove(playerName, out removedPlayer);
        }

        private void ValidateNotExistingPlayer(string playerName)
        {
            var existingPlayer = default(IGamePlayer);

            if (players.TryGetValue(playerName, out existingPlayer))
            {
                var errorMessage = string.Format("The player {0} is already connected", playerName);

                throw new ApplicationException(errorMessage);
            }
        }

        private void ValidateExistingPlayer(string playerName)
        {
            var existingPlayer = default(IGamePlayer);

            if (!players.TryGetValue(playerName, out existingPlayer))
            {
                var errorMessage = string.Format("The player {0} is not connected", playerName);

                throw new ApplicationException(errorMessage);
            }
        }

        private void ValidateExistingSession(string sessionId)
        {
            var existingSession = default(IGameSession);

            if (!sessions.TryGetValue(sessionId, out existingSession))
            {
                var errorMessage = string.Format("The session {0} does not exists", sessionId);

                throw new ApplicationException(errorMessage);
            }
        }

        private void ValidatePlayerInSession(string playerName, string sessionId)
        {
            this.ValidateExistingPlayer(playerName);
            this.ValidateExistingSession(sessionId);

            var existingSession = sessions
                .Where(s => s.Key == sessionId && s.Value.HasPlayer(playerName))
                .Select(s => s.Value);

            if (existingSession == null)
            {
                var errorMessage = string.Format("The player {0} is not part of the session {1}", playerName, sessionId);

                throw new ApplicationException(errorMessage);
            }
        }

        private ISessionGamePlayerBase GetRandomSessionPlayer2(ISessionGamePlayerBase sessionPlayer1)
        {
            var randomPlayer2 = players
                .Select(p => p.Value)
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
