namespace Gamify.Sdk
{
    public interface IGameSession
    {
        string Id { get; }

        bool IsReady { get; }

        ISessionGamePlayerBase Player1 { get; }

        ISessionGamePlayerBase Player2 { get; }

        void AddPlayer(ISessionGamePlayerBase player);

        ISessionGamePlayerBase GetPlayer(string playerName);

        void RemovePlayer(string playerName);

        bool HasPlayer(string playerName);
    }   
}
