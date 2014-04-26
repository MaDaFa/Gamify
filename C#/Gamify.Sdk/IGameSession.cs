namespace Gamify.Sdk
{
    public interface IGameSession
    {
        string Id { get; }

        bool IsReady { get; }

        IGamePlayer Player1 { get; }

        IGamePlayer Player2 { get; }

        void AddPlayer(IGamePlayer player);

        IGamePlayer GetPlayer(string playerName);

        void RemovePlayer(string playerName);

        bool HasPlayer(string playerName);
    }   
}
