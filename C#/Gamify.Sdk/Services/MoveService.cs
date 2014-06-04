using System;

namespace Gamify.Sdk.Services
{
    public class MoveService : IMoveService
    {
        private readonly ISessionService sessionService;

        public MoveService(ISessionService sessionService)
        {
            this.sessionService = sessionService;
        }

        public IGameMoveResponse<U> Handle<T, U>(string playerName, string sessionName, IGameMove<T> move)
        {
            var existingSession = this.sessionService.GetByName(sessionName);

            if (!existingSession.HasPlayer(playerName))
            {
                var errorMessage = string.Format("Player {0} does not belong to the session {1}", playerName, sessionName);

                throw new ApplicationException(errorMessage);
            }

            var playerToCall = existingSession.Player1.Information.UserName == playerName ? existingSession.Player2 : existingSession.Player1;

            return (playerToCall as ISessionGamePlayer<T, U>).ProcessMove(move);
        }
    }
}
