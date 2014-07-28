namespace Gamify.Sdk.Setup.Definition
{
    public interface IGameDefinition<TMove, UResponse>
    {
        ISessionPlayerFactory GetSessionPlayerFactory();

        ISessionPlayerSetup GetSessionPlayerSetup();

        IMoveFactory<TMove> GetMoveFactory();

        IMoveProcessor<TMove, UResponse> GetMoveProcessor();

        IMoveResultNotificationFactory GetMoveResultNotificationFactory();

        IGameInviteDecorator GetGameInviteDecorator();

        IPlayerHistoryItemFactory<TMove, UResponse> GetPlayerHistoryItemfactory();
    }
}
