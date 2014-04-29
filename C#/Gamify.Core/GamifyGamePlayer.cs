using Gamify.Sdk;

namespace Gamify.Service
{
    public class GamifyGamePlayer : IGamePlayer
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        public GamifyGamePlayer(string userName)
        {
            this.UserName = userName;
        }

        public GamifyGamePlayer()
        {
        }
    }
}
