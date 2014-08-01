namespace Gamify.Sdk.Contracts.ServerMessages
{
    public class GamifyServerMessageType
    {
        public const int GameInviteReceived = 400;
        public const int GameCreated = 401;
        public const int GameRejected = 402;
        public const int MoveReceived = 403;
        public const int MoveResultReceived = 404;
        public const int ActiveGamesList = 405;
        public const int PendingGamesList = 406;
        public const int FinishedGamesList = 407;
        public const int GameInformationReceived = 408;
        public const int GameAbandoned = 409;
        public const int GameFinished = 410;
    }
}
