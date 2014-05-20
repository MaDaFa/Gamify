namespace Gamify.Client.Net
{
    public class GameClientFactory
    {
        private static readonly string gameServerUrlTemplate = "ws://ec2-54-207-14-192.sa-east-1.compute.amazonaws.com/guessmynumber/api/guessmynumber?userName=?{0}";
        private static readonly object lockObject = new object();
        private static IGameClient gameClient;

        public static IGameClient GetGameClient(string playerName)
        {
            if (gameClient == null)
            {
                lock (lockObject)
                {
                    if (gameClient == null)
                    {
                        var gameServerUri = string.Format(gameServerUrlTemplate, playerName);

                        gameClient = new GameClient(gameServerUri);
                    }
                }
            }

            return gameClient;
        }
    }
}
