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
using System.Collections.Generic;
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
            this.webSocketServer = new WebSocketServer(configuration.Port, IPAddress.Any)
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

        protected abstract IEnumerable<ISessionGamePlayerBase> GetSessionPlayers(CreateGameRequestObject createGameRequestObject);

        protected abstract void GetSessionPlayer2Ready(GameAcceptedRequestObject gameAcceptedRequestObject);

        protected abstract void HandleGameMove(string serializedRequestObject, UserContext context);

        protected virtual void DecorateGameInvitation(GameInviteNotificationObject gameInviteNotificationObject)
        {
        }

        public string Start()
        {
            this.webSocketServer.Start();

            return this.webSocketServer.ListenAddress.ToString();
        }

        public void Stop()
        {
            this.webSocketServer.Stop();
        }

        public void Restart()
        {
            this.webSocketServer.Restart();
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
            if(context.DataFrame.IsHeartBeat())
            {
                return;
            }

            var jsonMessage = context.DataFrame.ToString();
            var gameRequest = JsonConvert.DeserializeObject<GameRequest>(jsonMessage);
            var gameRequestType = (GameRequestType)gameRequest.Type;

            switch (gameRequestType)
            {
                case GameRequestType.PlayerConnect:
                    this.ConnectPlayer(gameRequest, context);
                    break;
                case GameRequestType.CreateGame:
                    this.CreateGame(gameRequest);
                    break;
                case GameRequestType.GameAccepted:
                    this.GameAccepted(gameRequest);
                    break;
                case GameRequestType.GameMove:
                    this.HandleGameMove(gameRequest.SerializedRequestObject, context);
                    break;
                case GameRequestType.AbandonGame:
                    this.AbandonGame(gameRequest);
                    break;
                case GameRequestType.PlayerDisconnect:
                    this.DisconnectPlayer(gameRequest);
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

        private void ConnectPlayer(GameRequest request, UserContext context)
        {
            var playerConnectObject = JsonConvert.DeserializeObject<PlayerConnectRequestObject>(request.SerializedRequestObject);
            var newPlayer = new GamifyGamePlayer(playerConnectObject.PlayerName);
            var connectedClient = this.connectedClients
                .First(c => c.Key == context.ClientAddress.ToString())
                .Value;

            connectedClient.Player = newPlayer;

            this.gameController.Connect(newPlayer);

            this.SendPlayerConnectedNotification(newPlayer.UserName);
        }

        private void CreateGame(GameRequest request)
        {
            var createGameObject = JsonConvert.DeserializeObject<CreateGameRequestObject>(request.SerializedRequestObject);
            var sessionPlayers = this.GetSessionPlayers(createGameObject);
            var sessionPlayer1 = sessionPlayers.First(p => p.Information.UserName == createGameObject.PlayerName);
            var sessionPlayer2 = sessionPlayers.First(p => p.Information.UserName == createGameObject.InvitedPlayerName);
            var newSession = this.gameController.OpenSession(sessionPlayer1, sessionPlayer2);

            this.SendGameInviteNotification(newSession);
        }

        private void GameAccepted(GameRequest request)
        {
            var gameAcceptedObject = JsonConvert.DeserializeObject<GameAcceptedRequestObject>(request.SerializedRequestObject);

            this.GetSessionPlayer2Ready(gameAcceptedObject);

            var newSession = this.gameController.Sessions.First(s => s.Id == gameAcceptedObject.SessionId);

            newSession.Player1.NeedsToMove = true;

            this.SendGameCreatedNotification(newSession);
        }

        private void AbandonGame(GameRequest request)
        {
            var abandonGameObject = JsonConvert.DeserializeObject<AbandonGameRequestObject>(request.SerializedRequestObject);
            var currentSession = this.gameController.Sessions.First(s => s.Id == abandonGameObject.SessionId);

            this.gameController.AbandonSession(abandonGameObject.PlayerName, abandonGameObject.SessionId);

            this.SendAbandonGameNotification(abandonGameObject, currentSession);
        }

        private void DisconnectPlayer(GameRequest request)
        {
            var playerDisconnectObject = JsonConvert.DeserializeObject<PlayerDisconnectRequestObject>(request.SerializedRequestObject);

            this.gameController.Disconnect(playerDisconnectObject.PlayerName);

            this.SendPlayerDisconnectedNotification(playerDisconnectObject.PlayerName);
        }

        private void SendPlayerConnectedNotification(string playerName)
        {
            var notification = new PlayerConnectedNotificationObject
            {
                PlayerName = playerName
            };

            this.SendBroadcastNotification(GameNotificationType.PlayerConnected, notification);
        }

        private void SendGameInviteNotification(IGameSession newSession)
        {
            var gameInviteNotificationObject = new GameInviteNotificationObject
            {
                SessionId = newSession.Id,
                Player1Name = newSession.Player1.Information.UserName
            };

            this.DecorateGameInvitation(gameInviteNotificationObject);

            var client2 = this.connectedClients
                .First(c => c.Value.Player.UserName == newSession.Player2.Information.UserName)
                .Value;

            this.SendNotification(GameNotificationType.GameInvite, gameInviteNotificationObject, client2);
        }

        private void SendGameCreatedNotification(IGameSession newSession)
        {
            var notification = new GameCreatedNotificationObject
            {
                Player1Name = newSession.Player1.Information.UserName,
                Player2Name = newSession.Player2.Information.UserName
            };
            var client1 = this.connectedClients.First(c => c.Value.Player.UserName == newSession.Player1.Information.UserName).Value;
            var client2 = this.connectedClients.First(c => c.Value.Player.UserName == newSession.Player2.Information.UserName).Value;

            this.SendBroadcastNotification(GameNotificationType.GameCreated, notification, client1, client2);
        }

        private void SendAbandonGameNotification(AbandonGameRequestObject abandonGameObject, IGameSession currentSession)
        {
            var notification = new GameAbandonedNotificationObject
            {
                SessionId = abandonGameObject.SessionId,
                PlayerName = abandonGameObject.PlayerName
            };
            var client1 = this.connectedClients.First(c => c.Value.Player.UserName == currentSession.Player1.Information.UserName).Value;
            var client2 = this.connectedClients.First(c => c.Value.Player.UserName == currentSession.Player2.Information.UserName).Value;

            this.SendBroadcastNotification(GameNotificationType.GameAbandoned, notification, client1, client2);
        }

        private void SendPlayerDisconnectedNotification(string playerName)
        {
            var notification = new PlayerDisconnectedNotificationObject
            {
                PlayerName = playerName
            };

            this.SendBroadcastNotification(GameNotificationType.PlayerDisconnected, notification);
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
                Type = (int)gameNotificationType,
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
    }
}
