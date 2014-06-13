namespace Gamify.Sdk.Setup.Definition
{
    public interface ISessionPlayerFactory
    {
        SessionGamePlayer Create(IGamePlayer gamePlayer);
    }
}
