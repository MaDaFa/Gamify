using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Sdk.Services
{
    public class GameService : IGameService
    {
        private readonly ISerializer serializer;
        private readonly IList<IGameComponent> components;

        public event EventHandler<GameNotificationEventArgs> Notification;

        public IEnumerable<IGameComponentInformation> Components
        {
            get { return this.components; }
        }

        public GameService(ISerializer serializer)
        {
            this.serializer = serializer;
            this.components = new List<IGameComponent>();
        }

        public void RegisterComponent(IGameComponent component)
        {
            component.Notification += (sender, e) =>
            {
                var notificationHandler = this.Notification;

                if (notificationHandler != null)
                {
                    notificationHandler(this, e);
                }
            };

            this.components.Add(component);
        }

        public void Connect(string userName, string accessToken)
        {
            var playerConnectRequest = new PlayerConnectRequestObject
            {
                PlayerName = userName,
                AccessToken = accessToken
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.PlayerConnect,
                SerializedRequestObject = this.serializer.Serialize(playerConnectRequest)
            };
            var message = this.serializer.Serialize(gameRequest);

            this.Send(message);
        }

        ///<exception cref="GameServiceException">GameServiceException</exception>
        public void Send(string message)
        {
            var gameRequest = this.serializer.Deserialize<GameRequest>(message);
            var component = this.components.FirstOrDefault(c => c.CanHandleRequest(gameRequest));

            if (component == null)
            {
                var errorMessage = string.Format("There is no component registered to handle request type {0}", gameRequest.Type);

                this.SendErrorNotification(errorMessage, receiver: gameRequest.Sender);
            }

            try
            {
                component.HandleRequest(gameRequest);
            }
            catch (GameException gameEx)
            {
                this.SendErrorNotification(gameEx, receiver: gameRequest.Sender);
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("An unhandled error occurred. Details: {0}", ex.Message);

                this.SendErrorNotification(errorMessage, receiver: gameRequest.Sender);
            }
        }

        public void Disconnect(string userName)
        {
            var playerDisconnectRequest = new PlayerDisconnectRequestObject
            {
                PlayerName = userName
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.PlayerDisconnect,
                SerializedRequestObject = this.serializer.Serialize(playerDisconnectRequest)
            };
            var message = this.serializer.Serialize(gameRequest);

            this.Send(message);
        }

        private void SendErrorNotification(GameException gameException, string receiver)
        {
            this.SendErrorNotification(gameException.Message, receiver);
        }

        private void SendErrorNotification(string exceptionMessage, string receiver)
        {
            var errorNotificationObject = new ErrorNotificationObject
            {
                Message = exceptionMessage
            };
            var notification = new GameNotification
            {
                Type = (int)GameNotificationType.Error,
                SerializedNotificationObject = this.serializer.Serialize(errorNotificationObject)
            };

            var notificationHandler = this.Notification;

            if (notificationHandler != null)
            {
                notificationHandler(this, new GameNotificationEventArgs(receiver, notification));
            }
        }
    }
}
