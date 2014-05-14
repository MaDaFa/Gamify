using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using System;

namespace GuessMyNumber.Client.Net
{
    public abstract class GamifyService<TRequest, UNotification> : IGamifyService<TRequest, UNotification>
    {
        private readonly IGamifyClient gamifyClient;
        private readonly IGamifySerializer<TRequest> requestSerializer;
        private readonly IGamifySerializer<UNotification> notificationSerializer;

        public event EventHandler<GameNotificationEventArgs<UNotification>> NotificationReceived;

        public GamifyService(IGamifyClient gamifyClient)
        {
            this.gamifyClient = gamifyClient;

            if (!this.gamifyClient.IsInitialized)
            {
                this.gamifyClient.Initialize();
                this.gamifyClient.MessageReceived += (sender, args) =>
                {
                    this.OnMessageReceived(args);
                };
            }

            this.requestSerializer = new GamifySerializer<TRequest>();
            this.notificationSerializer = new GamifySerializer<UNotification>();
        }

        protected abstract bool CanHandleNotification(GameNotification notification);

        protected abstract void HandleNotification(GameNotification notification);

        protected abstract GameRequest GetGameRequest(string serializedRequestObject);

        public void Send(TRequest request)
        {
            var serializedGameRequest = this.requestSerializer.Serialize(request);
            var gameRequest = this.GetGameRequest(serializedGameRequest);

            this.gamifyClient.Send(gameRequest);
        }

        private void OnMessageReceived(MessageReceivedEventArgs args)
        {
            if (this.CanHandleNotification(args.NotificationObject))
            {
                this.HandleNotification(args.NotificationObject);
            }
        }
    }
}
