using Gamify.Core.Interfaces;

namespace Gamify.Core
{
    public class GamePlayer : IGamePlayer
    {
        public string UserName { get; set; }

        public string Name { get; set; }

        public GamePlayer(string userName)
        {
            this.UserName = userName;
        }

        public GamePlayer()
        {
        }
    }
}
