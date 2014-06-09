using Gamify.Sdk;

namespace Gamify.Sdk.Data.Entities
{
    public class GamePlayer : MongoEntity, IGamePlayer
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public GamePlayer(string userName)
        {
            this.Name = userName;
        }

        public GamePlayer()
        {
        }
    }
}
