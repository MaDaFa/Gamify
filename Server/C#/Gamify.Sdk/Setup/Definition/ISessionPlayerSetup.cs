using Gamify.Sdk.Contracts.ClientMessages;

namespace Gamify.Sdk.Setup.Definition
{
    public interface ISessionPlayerSetup
    {
        void GetPlayerReady(CreateGameClientMessage createGameRequest, SessionGamePlayer gamePlayer);

        void GetPlayerReady(AcceptGameClientMessage gameAcceptedRequest, SessionGamePlayer gamePlayer);
    }
}
