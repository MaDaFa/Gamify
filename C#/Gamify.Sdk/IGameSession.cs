using System;

namespace Gamify.Sdk
{
    public enum SessionState : int
    {
        Active = 1,
        Finished = 2,
        Abandoned = 3
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
