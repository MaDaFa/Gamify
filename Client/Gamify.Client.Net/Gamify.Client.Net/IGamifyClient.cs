using Gamify.Contracts.Requests;
using System;

namespace GuessMyNumber.Client.Net
{
    public interface IGamifyClient
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        bool IsInitialized { get; }

        void Initialize();

        void Send(GameRequest gameRequest);
    }
}
