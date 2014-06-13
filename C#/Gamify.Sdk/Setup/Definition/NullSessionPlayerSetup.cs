using Gamify.Sdk.Contracts.Requests;

namespace Gamify.Sdk.Setup.Definition
{
    public class NullSessionPlayerSetup : ISessionPlayerSetup
    {
        public void GetPlayerReady(CreateGameRequestObject createGameRequest, SessionGamePlayer gamePlayer)
        {
            return;
        }

        public void GetPlayerReady(GameAcceptedRequestObject gameAcceptedRequest, SessionGamePlayer gamePlayer)
        {
            return;
        }
    }
}
