using Gamify.Contracts.Requests;

namespace Gamify.Sdk.Setup
{
    public interface ISessionPlayerSetup
    {
        void GetPlayerReady(CreateGameRequestObject createGameRequest, ISessionGamePlayerBase gamePlayer);

        void GetPlayerReady(GameAcceptedRequestObject gameAcceptedRequest, ISessionGamePlayerBase gamePlayer);
    }
}
