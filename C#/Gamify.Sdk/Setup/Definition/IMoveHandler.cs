using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;

namespace Gamify.Sdk.Setup.Definition
{
    public interface IMoveHandler
    {
        IGameMoveResponse Handle(MoveRequestObject moveRequest, IMoveService moveService);
    }
}
