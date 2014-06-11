using System;

namespace Gamify.Sdk
{
    public enum SessionState : int
    {
        Pending = 1,
        Active = 2,
        Finished = 3,
        Abandoned = 4
    }

    public interface IGameSession
    {
        SessionState State { get; }

        string Name { get; }

        ISessionGamePlayerBase Player1 { get; }

        string Player1Name { get; }

        ISessionGamePlayerBase Player2 { get; }

        string Player2Name { get; }

        bool HasPlayer(string playerName);

        ISessionGamePlayerBase GetPlayer(string playerName);

        ISessionGamePlayerBase GetVersusPlayer(string playerName);
    }   
}
