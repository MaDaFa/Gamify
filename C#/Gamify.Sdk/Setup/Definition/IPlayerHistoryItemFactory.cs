using Gamify.Sdk.Contracts.Notifications;

namespace Gamify.Sdk.Setup.Definition
{
    public interface IPlayerHistoryItemFactory<TMove, UResponse>
    {
        IPlayerHistoryItem Create();
    }
}
