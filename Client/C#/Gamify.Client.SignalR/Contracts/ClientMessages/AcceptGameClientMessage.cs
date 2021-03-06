﻿using ThinkUp.Sdk.Contracts.ClientMessages;

namespace Gamify.Sdk.Contracts.ClientMessages
{
    public class AcceptGameClientMessage : IClientMessage
    {
        public string SessionName { get; set; }

        public string UserName { get; set; }

        public string AdditionalInformation { get; set; }
    }
}
