using Gamify.Sdk.Contracts.Notifications;

namespace Gamify.Sdk.Setup
{
    public interface IPlayerHistoryItemFactory<TMove, UResponse>
    {
        IPlayerHistoryItem Create();
    }
}
