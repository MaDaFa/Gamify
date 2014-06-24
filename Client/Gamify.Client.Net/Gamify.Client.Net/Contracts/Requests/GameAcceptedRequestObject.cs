using System;

namespace Gamify.Client.Net.Contracts.Requests
{
    public class GameAcceptedRequestObject : IRequestObject
    {
        public string SessionName { get; set; }

        public string PlayerName { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
