using Gamify.Sdk.Services;

namespace Gamify.Sdk.Setup
{
    public interface ISessionPlayerFactory
    {
        ISessionGamePlayerBase Create(IGamePlayer gamePlayer, ISessionHistoryService sessionHistoryService);
    }

    public interface ISessionPlayerFactory<TMove, UResponse> : ISessionPlayerFactory
    {
        SessionGamePlayer<TMove, UResponse> Create(IGamePlayer gamePlayer, ISessionHistoryService<TMove, UResponse> sessionHistoryService);
    }
}
