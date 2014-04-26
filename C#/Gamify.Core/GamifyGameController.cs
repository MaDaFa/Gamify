using Gamify.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Server
{
    public class GamifyGameController : IGameController
    {
        private readonly IList<IGamePlayer> players;
        private readonly IList<IGameSession> gameSessions;

        public IEnumerable<IGamePlayer> Players { get { return this.players; } }

        public IEnumerable<IGameSession> GameSessions { get { return this.gameSessions; } }

        public GamifyGameController()
        {
            this.players = new List<IGamePlayer>();
            this.gameSessions = new List<IGameSession>();
        }

        public void Connect(IGamePlayer player)
        {
            this.ValidateNotExistingPlayer(player.Name);

            this.players.Add(player);
        }

        public IGameSession OpenSession(string playerName, string versusPlayerName = null)
        {
            if (string.IsNullOrEmpty(versusPlayerName))
            {
                var versusPlayer = this.Players
                    .Where(p => !p.IsPlaying)
                    .OrderBy(p => Guid.NewGuid())
                    .FirstOrDefault(p => p.Name != playerName);

                if (versusPlayer == null)
                {
                    var errorMessage = "There are no users available to play";

                    throw new ApplicationException(errorMessage);
                }

                versusPlayerName = versusPlayer.Name;
            }

            this.ValidateAvailablePlayer(playerName);
            this.ValidateAvailablePlayer(versusPlayerName);

            var player1 = this.Players.FirstOrDefault(p => p.Name == playerName);
            var player2 = this.Players.FirstOrDefault(p => p.Name == versusPlayerName);

            (player1 as GamifyGamePlayer).IsPlaying = true;
            (player2 as GamifyGamePlayer).IsPlaying = true;

            var newSession = new GamifyGameSession(player1, player2);

            this.gameSessions.Add(newSession);

            return newSession;
        }

        public IGameMoveResponse<U> HandleMove<T, U>(string playerName, string sessionId, IGameMove<T> move)
        {
            this.ValidatePlayerInSession(playerName, sessionId);

            var existingSession = this.GameSessions.FirstOrDefault(s => s.Id == sessionId && s.HasPlayer(playerName));
            var playerToCall = existingSession.Player1.Name == playerName ? existingSession.Player2 : existingSession.Player1;

            return playerToCall.ProcessMove<T, U>(move);
        }

        public void AbandonSession(string playerName, string sessionId)
        {
            this.ValidatePlayerInSession(playerName, sessionId);

            var existingSession = this.GameSessions.FirstOrDefault(s => s.Id == sessionId && s.HasPlayer(playerName));

            existingSession.RemovePlayer(playerName);

            var player = this.Players.FirstOrDefault(p => p.Name == playerName);

            (player as GamifyGamePlayer).IsPlaying = false;
        }

        public void Disconnect(string playerName)
        {
            this.ValidateExistingPlayer(playerName);

            var existingSessions = this.GameSessions.Where(s => s.HasPlayer(playerName));

            foreach (var existingSession in existingSessions)
            {
                existingSession.RemovePlayer(playerName);
            }

            var player = this.Players.FirstOrDefault(p => p.Name == playerName);
            
            this.players.Remove(player);
        }

        private void ValidateNotExistingPlayer(string playerName)
        {
            if (this.Players.Any(p => p.Name == playerName))
            {
                var errorMessage = string.Format("The player {0} is already connected", playerName);

                throw new ApplicationException(errorMessage);
            }
        }

        private void ValidateExistingPlayer(string playerName)
        {
            if (!this.Players.Any(p => p.Name == playerName))
            {
                var errorMessage = string.Format("The player {0} is not connected", playerName);

                throw new ApplicationException(errorMessage);
            }
        }

        private void ValidateAvailablePlayer(string playerName)
        {
            this.ValidateExistingPlayer(playerName);

            if (this.Players.First(p => p.Name == playerName).IsPlaying)
            {
                var errorMessage = string.Format("The player {0} is not available", playerName);

                throw new ApplicationException(errorMessage);
            }
        }

        private void ValidateExistingSession(string sessionId)
        {
            if (!this.GameSessions.Any(s => s.Id == sessionId))
            {
                var errorMessage = string.Format("The session {0} does not exists", sessionId);

                throw new ApplicationException(errorMessage);
            }
        }

        private void ValidatePlayerInSession(string playerName, string sessionId)
        {
            this.ValidateExistingPlayer(playerName);
            this.ValidateExistingSession(sessionId);

            var existingSession = this.GameSessions.FirstOrDefault(s => s.Id == sessionId && s.HasPlayer(playerName));

            if (existingSession == null)
            {
                var errorMessage = string.Format("The player {0} is not part of the session {1}", playerName, sessionId);

                throw new ApplicationException(errorMessage);
            }
        }
    }
}
