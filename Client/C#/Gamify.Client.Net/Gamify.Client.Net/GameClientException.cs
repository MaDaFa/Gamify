using System;

namespace Gamify.Client.Net
{
    public class GameClientException : Exception
    {
        public GameClientException()
            : base()
        {
        }

        public GameClientException(string message)
            : base(message)
        {

        }

        public GameClientException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
