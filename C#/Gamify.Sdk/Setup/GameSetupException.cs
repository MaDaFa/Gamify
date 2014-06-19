using System;

namespace Gamify.Sdk.Setup
{
    public class GameSetupException : GameException
    {
        public GameSetupException()
        {
        }

        public GameSetupException(string message)
            : base(message)
        {
        }

        public GameSetupException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
