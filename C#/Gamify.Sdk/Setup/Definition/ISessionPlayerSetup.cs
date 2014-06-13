using Gamify.Sdk.Contracts.Requests;

namespace Gamify.Sdk.Setup.Definition
{
    public interface ISessionPlayerSetup
    {
        void GetPlayerReady(CreateGameRequestObject createGameRequest, SessionGamePlayer gamePlayer);

        void GetPlayerReady(GameAcceptedRequestObject gameAcceptedRequest, SessionGamePlayer gamePlayer);
    }
}
