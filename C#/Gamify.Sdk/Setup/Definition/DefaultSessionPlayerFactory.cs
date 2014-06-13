namespace Gamify.Sdk.Setup.Definition
{
    public class DefaultSessionPlayerFactory : ISessionPlayerFactory
    {
        public SessionGamePlayer Create(IGamePlayer gamePlayer)
        {
            return new SessionGamePlayer
            {
                Information = gamePlayer
            };
        }
    }
}
