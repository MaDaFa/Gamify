using System.Collections.Concurrent;
using System.Text;

namespace Gamify.Client.Net.Client
{
    public class GameClientFactory : IGameClientFactory
    {
        private readonly string gameServerUri;

        private static ConcurrentDictionary<string, IGameClient> gameClients;

        static GameClientFactory()
        {
            gameClients = new ConcurrentDictionary<string, IGameClient>();
        }

        public GameClientFactory(string gameServerUri)
        {
            if (gameServerUri.StartsWith("wss://"))
            {
                throw new GameClientException("Secure Web Sockets is not yet supported");
            }

            this.gameServerUri = gameServerUri;
        }

        public IGameClient GetGameClient(string playerName)
        {
            var gameClient = default(IGameClient);

            if (!gameClients.TryGetValue(playerName, out gameClient))
            {
                var completeGameServerUri = this.GetCompleteGameServerUri(playerName);

                gameClient = new GameClient(completeGameServerUri);

                gameClients.TryAdd(playerName, gameClient);
            }

            return gameClient;
        }

        private string GetCompleteGameServerUri(string playerName)
        {
            var gameServerUriBuilder = new StringBuilder();

            if (!this.gameServerUri.StartsWith("ws://"))
            {
                gameServerUriBuilder.Append("ws://");
            }

            gameServerUriBuilder.Append(this.gameServerUri);

            if (!this.gameServerUri.EndsWith("?userName="))
            {
                gameServerUriBuilder.Append("?userName=");
            }

            gameServerUriBuilder.Append(playerName);

            return gameServerUriBuilder.ToString();
        }
    }
}
