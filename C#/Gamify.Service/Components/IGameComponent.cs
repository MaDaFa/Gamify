using Gamify.Contracts.Requests;

namespace Gamify.Service.Components
{
    public interface IGameComponent
    {
        bool CanHandleRequest(GameRequest request);

        void HandleRequest(GameRequest request);
    }
}
