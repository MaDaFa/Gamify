using Gamify.Contracts.Requests;
using System;

namespace Gamify.Client.Net
{
    public interface IGameClient
    {
        event EventHandler<MessageReceivedEventArgs> MessageReceived;

        bool IsInitialized { get; }

        void Initialize();

        void Send(GameRequest gameRequest);
    }
}
