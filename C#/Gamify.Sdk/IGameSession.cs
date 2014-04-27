namespace Gamify.Sdk
{
    public interface IGameSession
    {
        string Id { get; }

        bool IsReady { get; }

        IGamePlayerBase Player1 { get; }

        IGamePlayerBase Player2 { get; }

        void AddPlayer(IGamePlayerBase player);

        IGamePlayerBase GetPlayer(string playerName);

        void RemovePlayer(string playerName);

        bool HasPlayer(string playerName);
    }   
}
