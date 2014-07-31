using System;

namespace Gamify.Sdk
{
    public class GameException: ApplicationException
    {
        public GameException()
        {
        }

        public GameException(string message)
            : base(message)
        {
        }

        public GameException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
