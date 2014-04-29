using Alchemy.Handlers.WebSocket;

namespace Gamify.Server
{
    public static class Extensions
    {
        public static bool IsHeartBeat(this DataFrame webSocketDataFrame)
        {
            return webSocketDataFrame.ToString() == "7";
        }
    }
}
