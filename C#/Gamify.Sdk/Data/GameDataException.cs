using System;

namespace Gamify.Sdk.Data
{
    public class GameDataException : ApplicationException
    {
        public GameDataException()
        {
        }

        public GameDataException(string message)
            : base(message)
        {
        }

        public GameDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
