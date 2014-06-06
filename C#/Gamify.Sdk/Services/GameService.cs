using Gamify.Sdk.Components;
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
            component.NotificationService.Notification += (sender, args) =>
            {
                var sendMessageHandler = this.Notification;

                if (sendMessageHandler != null)
                {
                    sendMessageHandler(this, args);
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

        public void Send(string message)
        {
            var gameRequest = this.serializer.Deserialize<GameRequest>(message);
            var component = this.components.FirstOrDefault(c => c.CanHandleRequest(gameRequest));

            if (component == null)
            {
                var errorMessage = string.Format("There is no component registered to handle request type {0}", gameRequest.Type);

                throw new ApplicationException(errorMessage);
            }

            component.HandleRequest(gameRequest);
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
    }
}
