using Gamify.Contracts.Requests;
using System;

namespace Gamify.Client.Net.Client
{
    public interface IGameClient
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        bool IsInitialized { get; }

        void Initialize();

        void Send(GameRequest gameRequest);
    }
}
