using ThinkUp.Sdk.Interfaces;

namespace Gamify.Sdk.Setup.Definition
{
    public class DefaultSessionPlayerFactory : ISessionPlayerFactory
    {
        public SessionGamePlayer Create(IUser gamePlayer)
        {
            return new SessionGamePlayer
            {
                Information = gamePlayer
            };
        }
    }
}
