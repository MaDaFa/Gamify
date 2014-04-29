using System;

namespace Gamify.Service
{
    public interface IGamifyService
    {
        event EventHandler<GamifyMessageEventArgs> SendMessage;

        void AddClient(string id, IGamifyClientBase client);

        bool HasClient(string id);

        void OnReceive(string clientId, string message);

        void OnDisconnect(string clientId);

        void OnError(string clientId, Exception ex);
    }
}
