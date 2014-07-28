using ThinkUp.Sdk.Contracts.ServerMessages;

namespace Gamify.Sdk.Contracts.ServerMessages
{
    public interface IMoveResultReceivedServerMessage : IServerMessage
    {
        string SessionName { get; set; }

        string PlayerName { get; set; }
    }
}
