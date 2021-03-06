﻿using ThinkUp.Sdk.Contracts.ClientMessages;

namespace Gamify.Sdk.Contracts.ClientMessages
{
    public class OpenGameClientMessage : IClientMessage
    {
        public string SessionName { get; set; }

        public string UserName { get; set; }
    }
}
