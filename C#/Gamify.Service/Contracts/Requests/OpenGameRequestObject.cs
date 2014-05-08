using System;

namespace Gamify.Service.Contracts.Requests
{
    public class OpenGameRequestObject : IRequestObject
    {
        public string SessionId { get; set; }

        public string PlayerName { get; set; }
    }
}
