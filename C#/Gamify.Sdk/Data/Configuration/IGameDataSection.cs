namespace Gamify.Sdk.Data.Configuration
{
    public interface IGameDataSection
    {
        string ConnectionString { get; }

        string DatabaseName { get; }
    }
}
