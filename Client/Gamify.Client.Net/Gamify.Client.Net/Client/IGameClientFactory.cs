namespace Gamify.Client.Net.Client
{
    public interface IGameClientFactory
    {
        IGameClient GetGameClient(string playerName);
    }
}
