namespace Gamify.Sdk
{
    public interface IGamePlayer
    {
        string Name { get; }

        string DisplayName { get; }

        bool IsConnected { get; }
    }
}
