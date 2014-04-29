using Gamify.Sdk;
using Gamify.Service.Contracts.Notifications;
using Gamify.Service.Contracts.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Service
{
    public abstract class GamifyService : IGamifyService
    {
        protected readonly ConcurrentDictionary<string, IGamifyClientBase> connectedClients;
        protected readonly IGameController gameController;

        public event EventHandler<GamifyMessageEventArgs> SendMessage;

        protected GamifyService(IGameController gameController)
        {
            this.connectedClients = new ConcurrentDictionary<string, IGamifyClientBase>();
            this.gameController = gameController;
        }

        protected abstract IEnumerable<ISessionGamePlayerBase> GetSessionPlayers(CreateGameRequestObject createGameRequestObject);

        protected abstract void GetSessionPlayer2Ready(GameAcceptedRequestObject gameAcceptedRequestObject);

        protected abstract void HandleGameMove(string clientId, string serializedRequestObject);

        protected virtual void DecorateGameInvitation(GameInviteNotificationObject gameInviteNotificationObject)
        {
        }

        public void AddClient(string id, IGamifyClientBase client)
        {
            if (this.HasClient(id))
            {
                return;
            }

            this.connectedClients.TryAdd(id, client);
        }

        public bool HasClient(string id)
        {
            return this.connectedClients.Any(c => c.Key == id);
        }

        public void OnReceive(string clientId, string message)
        {
            var gameRequest = JsonConvert.DeserializeObject<GameRequest>(message);
            var gameRequestType = (GameRequestType)gameRequest.Type;

            switch (gameRequestType)
            {
                case GameRequestType.PlayerConnect:
                    this.ConnectPlayer(clientId, gameRequest);
                    break;
                case GameRequestType.CreateGame:
                    this.CreateGame(gameRequest);
                    break;
                case GameRequestType.GameAccepted:
                    this.GameAccepted(gameRequest);
                    break;
                case GameRequestType.GameMove:
                    this.HandleGameMove(clientId, gameRequest.SerializedRequestObject);
                    break;
                case GameRequestType.AbandonGame:
                    this.AbandonGame(gameRequest);
                    break;
                case GameRequestType.PlayerDisconnect:
                    this.DisconnectPlayer(gameRequest);
                    break;
            }
        }

        public void OnDisconnect(string clientId)
        {
            var connectedClient = default(IGamifyClientBase);

            this.connectedClients.TryRemove(clientId, out connectedClient);
        }

        public void OnError(string clientId, Exception ex)
        {
            this.SendErrorNotification(clientId, ex);
        }

        private void ConnectPlayer(string clientId, GameRequest request)
        {
            var playerConnectObject = JsonConvert.DeserializeObject<PlayerConnectRequestObject>(request.SerializedRequestObject);
            var newPlayer = new GamifyGamePlayer(playerConnectObject.PlayerName);
            var connectedClient = this.connectedClients
                .First(c => c.Key == clientId)
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

        private void SendErrorNotification(string clientId, Exception exception)
        {
            var client = this.connectedClients.First(c => c.Key == clientId).Value;
            var notification = new ErrorNotificationObject
            {
                Message = exception.Message
            };

            this.SendBroadcastNotification(GameNotificationType.Error, notification, client);
        }

        protected void SendBroadcastNotification(GameNotificationType gameNotificationType, object notificationObject)
        {
            this.SendBroadcastNotification(gameNotificationType, notificationObject, this.connectedClients.Values.ToArray());
        }

        protected void SendBroadcastNotification(GameNotificationType gameNotificationType, object notificationObject, params IGamifyClientBase[] clients)
        {
            foreach (var connectedClient in clients)
            {
                this.SendNotification(gameNotificationType, notificationObject, connectedClient);
            }
        }

        protected void SendNotification(GameNotificationType gameNotificationType, object notificationObject, IGamifyClientBase client)
        {
            var notification = new GameNotification
            {
                Type = (int)gameNotificationType,
                SerializedNotificationObject = JsonConvert.SerializeObject(notificationObject)
            };
            var serializedNotification = JsonConvert.SerializeObject(notification);
            var sendMessageHandler = this.SendMessage;

            if (sendMessageHandler != null)
            {
                sendMessageHandler(this, new GamifyMessageEventArgs(client, serializedNotification));
            }
        }
    }
}
