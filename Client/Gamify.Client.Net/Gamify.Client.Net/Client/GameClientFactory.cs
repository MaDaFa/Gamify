using System.Text;

namespace Gamify.Client.Net.Client
{
    public class GameClientFactory : IGameClientFactory
    {
        private readonly string gameServerUri;

        public GameClientFactory(string gameServerUri)
        {
            if (gameServerUri.StartsWith("wss://"))
            {
                throw new GameClientException("Secure Web Sockets is not yet supported");
            }

            this.gameServerUri = gameServerUri;
        }

        public IGameClient Create()
        {
            var completeGameServerUri = this.GetCompleteGameServerUri();

            return new GameClient(completeGameServerUri);
        }

        private string GetCompleteGameServerUri()
        {
            var gameServerUriBuilder = new StringBuilder();

            if (!this.gameServerUri.StartsWith("ws://"))
            {
                gameServerUriBuilder.Append("ws://");
            }

            gameServerUriBuilder.Append(this.gameServerUri);

            return gameServerUriBuilder.ToString();
        }
    }
}
