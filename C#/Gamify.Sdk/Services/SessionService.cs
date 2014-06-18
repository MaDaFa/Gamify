using Gamify.Sdk.Data;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Setup.Definition;
using System.Collections.Generic;

namespace Gamify.Sdk.Services
{
    public class SessionService : ISessionService
    {
        private readonly IPlayerService playerService;
        private readonly IRepository<GameSession> sessionRepository;
        private readonly ISessionPlayerFactory sessionPlayerFactory;

        public SessionService(IPlayerService playerService, IRepository<GameSession> sessionRepository, 
            ISessionPlayerFactory sessionPlayerFactory)
        {
            this.playerService = playerService;
            this.sessionRepository = sessionRepository;
            this.sessionPlayerFactory = sessionPlayerFactory;
        }

        public IEnumerable<IGameSession> GetAll()
        {
            return this.sessionRepository.GetAll();
        }

        public IEnumerable<IGameSession> GetAllByPlayer(string playerName)
        {
            return this.sessionRepository.GetAll(s => s.Player1Name == playerName || s.Player2Name == playerName);
        }

        public IEnumerable<IGameSession> GetPendings(string playerName)
        {
            return this.sessionRepository.GetAll(s => (s.Player1Name == playerName || s.Player2Name == playerName)
                && s.State == SessionState.Pending);
        }

        public IEnumerable<IGameSession> GetActives(string playerName)
        {
            return this.sessionRepository.GetAll(s => (s.Player1Name == playerName || s.Player2Name == playerName)
                && s.State == SessionState.Active);
        }

        public IEnumerable<IGameSession> GetFinished(string playerName)
        {
            return this.sessionRepository.GetAll(s => (s.Player1Name == playerName || s.Player2Name == playerName)
                && s.State == SessionState.Finished);
        }

        public IGameSession GetByName(string sessionName)
        {
            return this.sessionRepository.Get(s => s.Name == sessionName);
        }

        ///<exception cref="GameServiceException">GameServiceException</exception>
        public IGameSession Create(SessionGamePlayer sessionPlayer1, SessionGamePlayer sessionPlayer2 = null)
        {
            if (sessionPlayer2 == null)
            {
                sessionPlayer2 = this.GetRandomSessionPlayer2(sessionPlayer1);
            }

            var newSession = new GameSession(sessionPlayer1, sessionPlayer2);
            var existSession = this.sessionRepository.Exist(s => s.State == SessionState.Active && s.Name == newSession.Name);

            if (existSession)
            {
                var errorMessage = string.Format("There is already an active session for players {0} and {1}", sessionPlayer1.Information.Name, sessionPlayer2.Information.Name);

                throw new GameServiceException(errorMessage);
            }

            try
            {
                this.sessionRepository.Create(newSession);
            }
            catch (GameDataException gameDataEx)
            {
                var errorMessage = string.Format("An error occured when creating the session {0}", newSession.Name);

                throw new GameServiceException(errorMessage, gameDataEx);
            }

            return newSession;
        }

        ///<exception cref="GameServiceException">GameServiceException</exception>
        public void Start(IGameSession currentSession)
        {
            var sessionToUpdate = currentSession as GameSession;

            sessionToUpdate.State = SessionState.Active;

            try
            {
                this.sessionRepository.Update(sessionToUpdate);
            }
            catch (GameDataException gameDataEx)
            {
                var errorMessage = string.Format("An error occured when starting the session {0}", sessionToUpdate.Name);

                throw new GameServiceException(errorMessage, gameDataEx);
            }
        }

        ///<exception cref="GameServiceException">GameServiceException</exception>
        public void Abandon(string sessionName)
        {
            var existingSession = this.sessionRepository.Get(s => s.State == SessionState.Active && s.Name == sessionName);

            if (existingSession == null)
            {
                var errorMessage = string.Format("There is no active session {0} to abandon", sessionName);

                throw new GameServiceException(errorMessage);
            }

            existingSession.State = SessionState.Abandoned;

            try
            {
                this.sessionRepository.Update(existingSession);
            }
            catch (GameDataException gameDataEx)
            {
                var errorMessage = string.Format("An error occured when trying to abandon the session {0}", existingSession.Name);

                throw new GameServiceException(errorMessage, gameDataEx);
            }
        }

        ///<exception cref="GameServiceException">GameServiceException</exception>
        public void Finish(string sessionName)
        {
            var existingSession = this.sessionRepository.Get(s => s.State == SessionState.Active && s.Name == sessionName);

            if (existingSession == null)
            {
                var errorMessage = string.Format("There is no active session {0} to finish", sessionName);

                throw new GameServiceException(errorMessage);
            }

            existingSession.State = SessionState.Finished;

            try
            {
                this.sessionRepository.Update(existingSession);
            }
            catch (GameDataException gameDataEx)
            {
                var errorMessage = string.Format("An error occured when trying to finish the session {0}", existingSession.Name);

                throw new GameServiceException(errorMessage, gameDataEx);
            }
        }

        private SessionGamePlayer GetRandomSessionPlayer2(SessionGamePlayer sessionPlayer1)
        {
            var randomPlayer2 = this.playerService.GetRandom(playerNameToExclude: sessionPlayer1.Information.Name);

            if (randomPlayer2 == null)
            {
                var errorMessage = "There are no users available to play";

                throw new GameServiceException(errorMessage);
            }

            return this.sessionPlayerFactory.Create(randomPlayer2);
        }
    }
}
