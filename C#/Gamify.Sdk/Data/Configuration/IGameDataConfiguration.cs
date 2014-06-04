namespace Gamify.Sdk.Data.Configuration
{
    public interface IGameDataConfiguration
    {
        string ConnectionString { get; }

        string DatabaseName { get; }
    }
}
