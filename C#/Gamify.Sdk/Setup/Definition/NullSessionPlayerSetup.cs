using Gamify.Sdk.Contracts.ClientMessages;

namespace Gamify.Sdk.Setup.Definition
{
    public class NullSessionPlayerSetup : ISessionPlayerSetup
    {
        public void GetPlayerReady(CreateGameClientMessage createGameRequest, SessionGamePlayer gamePlayer)
        {
            return;
        }

        public void GetPlayerReady(AcceptGameClientMessage gameAcceptedRequest, SessionGamePlayer gamePlayer)
        {
            return;
        }
    }
}
