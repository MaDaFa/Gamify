﻿using System;

namespace Gamify.Client.Net.Contracts.Requests
{
    public class OpenGameRequestObject : IRequestObject
    {
        public string SessionName { get; set; }

        public string PlayerName { get; set; }
    }
}
