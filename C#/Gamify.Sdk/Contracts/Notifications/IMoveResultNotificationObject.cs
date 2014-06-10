namespace Gamify.Sdk.Contracts.Notifications
{
    public interface IMoveResultNotificationObject : INotificationObject
    {
        string SessionName { get; set; }

        string PlayerName { get; set; }
    }
}
