using Gamify.Contracts.Notifications;
using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Core.Interfaces;
using Gamify.Service.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Service
{
    public class GameService : IGameService
    {
        private readonly ISerializer<GameRequest> serializer;
        private readonly IList<IGameComponent> components;
        private readonly INotificationService notificationService;
        private readonly IGameController gameController;

        public event EventHandler<GameNotificationEventArgs> Notification;

        public GameService(INotificationService notificationService, IGameController gameController)
        {
            this.serializer = new JsonSerializer<GameRequest>();
            this.components = new List<IGameComponent>();
            this.notificationService = notificationService;
            this.gameController = gameController;

            this.notificationService.Notification += (sender, args) =>
            {
                var sendMessageHandler = this.Notification;

                if (sendMessageHandler != null)
                {
                    sendMessageHandler(this, args);
                }
            };
        }

        public void RegisterComponent(IGameComponent component)
        {
            this.components.Add(component);
        }

        public void ConnectUser(string userName)
        {
            var newPlayer = new GamePlayer(userName);
            var notification = new PlayerConnectedNotificationObject
            {
                PlayerName = userName
            };
            var playersToNotify = this.gameController.Players
                .Where(p => p.UserName != userName)
                .Select(p => p.UserName);

            this.gameController.Connect(newPlayer);
            this.notificationService.SendBroadcast(GameNotificationType.PlayerConnected, notification, playersToNotify.ToArray());
        }

        public void ReceiveMessage(string message)
        {
            var gameRequest = this.serializer.Deserialize(message);
            var component = this.components.FirstOrDefault(c => c.CanHandleRequest(gameRequest));

            if (component == null)
            {
                var errorMessage = string.Format("There is no component registered to handle request type {0}", gameRequest.Type);

                throw new ApplicationException(errorMessage);
            }

            component.HandleRequest(gameRequest);
        }

        public void DisconnectUser(string userName)
        {
            var notification = new PlayerDisconnectedNotificationObject
            {
                PlayerName = userName
            };
            var playersToNotify = this.gameController.Players
                .Where(p => p.UserName != userName)
                .Select(p => p.UserName);

            this.gameController.Disconnect(userName);
            this.notificationService.SendBroadcast(GameNotificationType.PlayerDisconnected, notification, playersToNotify.ToArray());
        }

        private void SendErrorNotification(string userName, Exception exception)
        {
            var notification = new ErrorNotificationObject
            {
                Message = exception.Message
            };

            this.notificationService.Send(GameNotificationType.Error, notification, userName);
        }
    }
}
