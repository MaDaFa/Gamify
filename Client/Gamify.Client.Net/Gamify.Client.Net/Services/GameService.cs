using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using System;

namespace Gamify.Client.Net.Services
{
    public class GameService<TRequest, UNotification> : IGameService<TRequest, UNotification>
        where TRequest : IRequestObject
        where UNotification : INotificationObject
    {
        private readonly GameRequestType requestType;
        private readonly GameNotificationType notificationType;
        private readonly IGameClient gamifyClient;
        private readonly ISerializer<TRequest> requestSerializer;
        private readonly ISerializer<UNotification> notificationSerializer;

        public event EventHandler<GameNotificationEventArgs<UNotification>> NotificationReceived;

        public GameService(GameRequestType requestType, GameNotificationType notificationType, IGameClient gamifyClient)
        {
            this.requestType = requestType;
            this.notificationType = notificationType;
            this.gamifyClient = gamifyClient;

            if (!this.gamifyClient.IsInitialized)
            {
                this.gamifyClient.Initialize();
                this.gamifyClient.MessageReceived += (sender, args) =>
                {
                    this.OnMessageReceived(args);
                };
            }

            this.requestSerializer = new JsonSerializer<TRequest>();
            this.notificationSerializer = new JsonSerializer<UNotification>();
        }

        public void Send(TRequest request)
        {
            var serializedGameRequest = this.requestSerializer.Serialize(request);
            var gameRequest = new GameRequest
            {
                Type = (int)this.requestType,
                SerializedRequestObject = serializedGameRequest
            };

            this.gamifyClient.Send(gameRequest);
        }

        private bool CanParseNotification(GameNotification notification)
        {
            return (GameNotificationType)notification.Type == this.notificationType;
        }

        private UNotification ParseNotification(GameNotification notification)
        {
            var notificationObject = this.notificationSerializer.Deserialize(notification.SerializedNotificationObject);

            return notificationObject;
        }

        private void OnMessageReceived(MessageReceivedEventArgs args)
        {
            if (this.CanParseNotification(args.GameNotification))
            {
                var notificationObject = this.ParseNotification(args.GameNotification);

                if (this.NotificationReceived != null)
                {
                    this.NotificationReceived(this, new GameNotificationEventArgs<UNotification>(notificationObject));
                }
            }
        }
    }
}
