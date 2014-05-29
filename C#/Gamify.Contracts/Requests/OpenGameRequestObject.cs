using System;

namespace Gamify.Contracts.Requests
{
    public class OpenGameRequestObject : IRequestObject
    {
        public string SessionName { get; set; }

        public string PlayerName { get; set; }
    }
}
