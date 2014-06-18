using System;

namespace Gamify.Sdk.Services
{
    public class GameServiceException : GameException
    {
        public GameServiceException()
        {
        }

        public GameServiceException(string message)
            : base(message)
        {
        }

        public GameServiceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
