using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Setup
{
    public interface IMoveHandler
    {
        IGameMoveResponse Handle(MoveRequestObject moveRequest, IMoveService moveService);
    }
}
