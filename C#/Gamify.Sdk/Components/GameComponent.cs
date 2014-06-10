using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Services;
using System;

namespace Gamify.Sdk.Components
{
    public abstract class GameComponent : IGameComponent
    {
        protected readonly INotificationService notificationService;

        public event EventHandler<GameNotificationEventArgs> Notification;

        public string Name
        {
            get { return this.GetType().Name; }
        }

        public GameComponent(INotificationService notificationService)
        {
            this.notificationService = notificationService;
            this.notificationService.Notification += (sender, e) =>
            {
                if (this.CanHandleNotification(e.Notification))
                {
                    var notificationHandler = this.Notification;

                    if (notificationHandler != null)
                    {
                        notificationHandler(this, e);
                    }
                }
            };
        }

        public abstract bool CanHandleRequest(GameRequest request);

        public abstract bool CanHandleNotification(GameNotification notification);

        public abstract void HandleRequest(GameRequest request);
    }
}
