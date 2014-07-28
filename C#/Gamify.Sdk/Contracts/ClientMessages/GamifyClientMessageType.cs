namespace Gamify.Sdk.Contracts.ClientMessages
{
    public class GamifyClientMessageType
    {
        public const int CreateGame = 300;
        public const int AcceptGame = 301;
        public const int RejectGame = 302;
        public const int SendMove = 303;
        public const int AbandonGame = 304;
        public const int GetActiveGames = 305;
        public const int GetPendingGames = 306;
        public const int GetFinishedGames = 307;
        public const int OpenGame = 308;
    }
}
