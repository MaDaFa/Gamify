using Alchemy.Classes;
using Gamify.Sdk;

namespace Gamify.Server
{
    public class GamifyClient
    {
        public UserContext Context { get; set; }

        public IGamePlayer Player { get; set; }
    }
}
