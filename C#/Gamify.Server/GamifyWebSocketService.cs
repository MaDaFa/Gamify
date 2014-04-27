using Alchemy;
using Alchemy.Classes;
using Gamify.Sdk;
using Gamify.Server;
using Gamify.Server.Configuration;
using Gamify.Server.Contracts.Notifications;
using Gamify.Server.Contracts.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;

namespace WebSocketsTest.Server.Services
{
    public abstract class GamifyWebSocketService
    {
        protected readonly WebSocketServer webSocketServer;
        protected readonly ConcurrentDictionary<string, GamifyClient> connectedClients;
        protected readonly IGameController gameController;

        protected GamifyWebSocketService(IGamifyConfiguration configuration, IGameController gameController)
        {
            var ipAddress = IPAddress.Parse(configuration.IpAddress);

            this.webSocketServer = new WebSocketServer(configuration.Port, ipAddress)
            {
                OnConnected = OnConnect,
                OnReceive = OnReceive,
                OnSend = OnSend,
                OnDisconnect = OnDisconnect,
                TimeOut = new TimeSpan(configuration.TimeoutHours, configuration.TimeoutMinutes, configuration.TimeoutSeconds)
            };
            this.connectedClients = new ConcurrentDictionary<string, GamifyClient>();
            this.gameController = gameController;
        }

        public void OnConnect(UserContext context)
		{
            this.ValidateExistingClient(context);

            this.connectedClients.TryAdd(context.ClientAddress.ToString(), new GamifyClient
            {
                Context = context
            });
		}

        public void OnReceive(UserContext context)
        {
            var jsonMessage = context.DataFrame.ToString();
            var gameRequest = JsonConvert.DeserializeObject<GameRequest>(jsonMessage);

            switch (gameRequest.Type)
            {
                case GameRequestType.UserConnect:
                    this.ConnectUser(gameRequest, context);
                    break;
                case GameRequestType.OpenSession:
                    this.OpenSession(gameRequest);
                    break;
                case GameRequestType.GameMove:
                    this.HandleGameMove(gameRequest.SerializedRequestObject, context);
                    break;
                case GameRequestType.AbandonSession:
                    this.AbandonSession(gameRequest);
                    break;
                case GameRequestType.UserDisconnect:
                    this.DisconnectUser(gameRequest);
                    break;
            }
        }

        public virtual void OnSend(UserContext context)
        {
        }

        public void OnDisconnect(UserContext context)
        {
            var connectedClient = default(GamifyClient);

            this.connectedClients.TryRemove(context.ClientAddress.ToString(), out connectedClient);
        }

        protected abstract IGamePlayerBase GetNewPlayer(UserConnectRequestObject userConnectObject, UserContext context);

        protected abstract void HandleGameMove(string serializedRequestObject, UserContext context);

        protected virtual void PreOpenSession(OpenSessionRequestObject openSessionRequestObject)
        {
        }

        protected virtual void PostOpenSession(SessionOpenedNotificationObject notification)
        {
        }

        protected void SendBroadcastNotification(GameNotificationType gameNotificationType, object notificationObject)
        {
            this.SendBroadcastNotification(gameNotificationType, notificationObject, this.connectedClients.Values.ToArray());
        }

        protected void SendBroadcastNotification(GameNotificationType gameNotificationType, object notificationObject, params GamifyClient[] clients)
        {
            foreach (var connectedClient in clients)
            {
                this.SendNotification(gameNotificationType, notificationObject, connectedClient);
            }
        }

        protected void SendNotification(GameNotificationType gameNotificationType, object notificationObject, GamifyClient client)
        {
            var notification = new GameNotification
            {
                Type = gameNotificationType,
                SerializedNotificationObject = JsonConvert.SerializeObject(notificationObject)
            };
            var serializedNotification = JsonConvert.SerializeObject(notification);

            client.Context.Send(serializedNotification);
        }

        private void ValidateExistingClient(UserContext context)
        {
            if (this.connectedClients.Any(c => c.Key == context.ClientAddress.ToString()))
            {
                var errorMessage = string.Format("The machine {0} is already connected", context.ClientAddress);

                throw new ApplicationException(errorMessage);
            }
        }

        private void ConnectUser(GameRequest request, UserContext context)
        {
            var userConnectObject = JsonConvert.DeserializeObject<UserConnectRequestObject>(request.SerializedRequestObject);
            var newPlayer = this.GetNewPlayer(userConnectObject, context);
            var connectedClient = this.connectedClients
                .First(c => c.Key == context.ClientAddress.ToString())
                .Value;

            connectedClient.Player = newPlayer;

            var notification = new UserConnectedNotificationObject
            {
                PlayerName = newPlayer.Name
            };

            this.SendBroadcastNotification(GameNotificationType.UserConnected, notification);
        }

        private void OpenSession(GameRequest request)
        {
            var openSessionObject = JsonConvert.DeserializeObject<OpenSessionRequestObject>(request.SerializedRequestObject);

            this.PreOpenSession(openSessionObject);

            var newSession = this.gameController.OpenSession(openSessionObject.PlayerName, openSessionObject.VersusPlayerName);

            var notification = new SessionOpenedNotificationObject
            {
                Player1Name = newSession.Player1.Name,
                Player2Name = newSession.Player2.Name
            };
            var client1 = this.connectedClients.First(c => c.Value.Player.Name == newSession.Player1.Name).Value;
            var client2 = this.connectedClients.First(c => c.Value.Player.Name == newSession.Player2.Name).Value;

            this.PostOpenSession(notification);

            this.SendBroadcastNotification(GameNotificationType.UserConnected, notification, client1, client2);
        }

        private void AbandonSession(GameRequest request)
        {
            var abandonSessionObject = JsonConvert.DeserializeObject<AbandonSessionRequestObject>(request.SerializedRequestObject);
            var currentSession = this.gameController.GameSessions.First(s => s.Id == abandonSessionObject.SessionId);

            this.gameController.AbandonSession(abandonSessionObject.PlayerName, abandonSessionObject.SessionId);

            var notification = new SessionAbandonedNotificationObject
            {
                SessionId = abandonSessionObject.SessionId,
                PlayerName = abandonSessionObject.PlayerName
            };
            var client1 = this.connectedClients.First(c => c.Value.Player.Name == currentSession.Player1.Name).Value;
            var client2 = this.connectedClients.First(c => c.Value.Player.Name == currentSession.Player2.Name).Value;

            this.SendBroadcastNotification(GameNotificationType.SessionAbandoned, notification, client1, client2);
        }

        private void DisconnectUser(GameRequest request)
        {
            var userDisconnectObject = JsonConvert.DeserializeObject<UserDisconnectRequestObject>(request.SerializedRequestObject);

            this.gameController.Disconnect(userDisconnectObject.PlayerName);

            var notification = new UserDisconnectedNotificationObject
            {
                PlayerName = userDisconnectObject.PlayerName
            };

            this.SendBroadcastNotification(GameNotificationType.UserDisconnected, notification);
        }
    }
}
