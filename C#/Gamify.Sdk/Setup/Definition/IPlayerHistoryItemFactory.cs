using Gamify.Sdk.Contracts.ServerMessages;

namespace Gamify.Sdk.Setup.Definition
{
    public interface IPlayerHistoryItemFactory<TMove, UResponse>
    {
        IPlayerHistoryItem Create(UResponse gameResponseObject);
    }
}
