using Gamify.Sdk.Contracts.Requests;

namespace Gamify.Sdk.Setup.Definition
{
    public class NullSessionPlayerSetup : ISessionPlayerSetup
    {
        public void GetPlayerReady(CreateGameRequestObject createGameRequest, ISessionGamePlayerBase gamePlayer)
        {
            return;
        }

        public void GetPlayerReady(GameAcceptedRequestObject gameAcceptedRequest, ISessionGamePlayerBase gamePlayer)
        {
            return;
        }
    }
}
