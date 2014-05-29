using Gamify.Contracts.Requests;
using Gamify.Core;
using Gamify.Service.Components;
using Gamify.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Service
{
    public class GameService : IGameService
    {
        private readonly ISerializer<GameRequest> gameSerializer;
        private readonly IList<IGameComponent> components;

        public event EventHandler<GameNotificationEventArgs> Notification;

        public GameService()
        {
            this.gameSerializer = new JsonSerializer<GameRequest>();
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
            var playerConnectSerializer = new JsonSerializer<PlayerConnectRequestObject>();
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.PlayerConnect,
                SerializedRequestObject = playerConnectSerializer.Serialize(playerConnectRequest)
            };
            var message = this.gameSerializer.Serialize(gameRequest);

            this.Send(message);
        }

        public void Send(string message)
        {
            var gameRequest = this.gameSerializer.Deserialize(message);
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
            var playerDisconnectSerializer = new JsonSerializer<PlayerDisconnectRequestObject>();
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.PlayerDisconnect,
                SerializedRequestObject = playerDisconnectSerializer.Serialize(playerDisconnectRequest)
            };
            var message = this.gameSerializer.Serialize(gameRequest);

            this.Send(message);
        }
    }
}
