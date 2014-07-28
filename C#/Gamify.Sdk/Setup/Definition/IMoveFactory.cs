using Gamify.Sdk.Interfaces;

namespace Gamify.Sdk.Setup.Definition
{
    public interface IMoveFactory<T>
    {
        IGameMove<T> Create(string moveInformation);
    }
}
