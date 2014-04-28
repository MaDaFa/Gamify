using System;

namespace Gamify.Server.Contracts.Requests
{
    public class GameAcceptedRequestObject : IRequestObject
    {
        public string SessionId { get; set; }

        public string PlayerName { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
