using Gamify.Core;

namespace Gamify.Data.Entities
{
    public class GamePlayer : MongoEntity, IGamePlayer
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
