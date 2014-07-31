using ThinkUp.Sdk.Interfaces;

namespace Gamify.Sdk.Setup.Definition
{
    public interface ISessionPlayerFactory
    {
        SessionGamePlayer Create(IUser gamePlayer);
    }
}
