using Gamify.Sdk.Contracts.Requests;

namespace Gamify.Sdk.Setup.Definition
{
    public interface ISessionPlayerSetup
    {
        void GetPlayerReady(CreateGameRequestObject createGameRequest, ISessionGamePlayerBase gamePlayer);

        void GetPlayerReady(GameAcceptedRequestObject gameAcceptedRequest, ISessionGamePlayerBase gamePlayer);
    }
}
