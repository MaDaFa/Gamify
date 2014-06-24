using Gamify.Client.Net.Client;
using Gamify.Client.Net.Contracts.Notifications;
using System;

namespace Gamify.Client.Net.Services
{
    public class GameListener<TNotification> : IGameListener<TNotification>
        where TNotification : INotificationObject
    {
        private readonly ISerializer serializer;
        private readonly GameNotificationType notificationType;
        protected readonly IGameClient gameClient;

        public event EventHandler<GameNotificationEventArgs<TNotification>> NotificationReceived;

        public GameListener(GameNotificationType notificationType, IGameClient gameClient)
        {
            this.serializer = new JsonSerializer();

            this.notificationType = notificationType;
            this.gameClient = gameClient;

            if (!this.gameClient.IsInitialized)
            {
                this.gameClient.Initialize();
                this.gameClient.MessageReceived += (sender, args) =>
                {
                    this.OnMessageReceived(args);
                };
            }
        }

        private bool CanParseNotification(GameNotification notification)
        {
            return (GameNotificationType)notification.Type == this.notificationType;
        }

        private TNotification ParseNotification(GameNotification notification)
        {
            var notificationObject = this.serializer.Deserialize<TNotification>(notification.SerializedNotificationObject);

            return notificationObject;
        }

        private void OnMessageReceived(MessageReceivedEventArgs args)
        {
            if (this.CanParseNotification(args.GameNotification))
            {
                var notificationObject = this.ParseNotification(args.GameNotification);

                if (this.NotificationReceived != null)
                {
                    this.NotificationReceived(this, new GameNotificationEventArgs<TNotification>(notificationObject));
                }
            }
        }
    }
}
