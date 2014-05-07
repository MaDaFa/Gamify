using Gamify.Sdk;
using Gamify.Service.Contracts.Notifications;
using Gamify.Service.Contracts.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Service
{
    public abstract class GamifyService : IGamifyService
    {
        protected readonly IGameController gameController;

        public event EventHandler<GamifyMessageEventArgs> SendMessage;

        protected GamifyService(IGameController gameController)
        {
            this.gameController = gameController;
        }

        protected abstract IEnumerable<ISessionGamePlayerBase> GetSessionPlayers(CreateGameRequestObject createGameRequestObject);

        protected abstract void GetSessionPlayer2Ready(GameAcceptedRequestObject gameAcceptedRequestObject);

        protected abstract void HandleGameMove(string serializedRequestObject);

        protected virtual void DecorateGameInvitation(GameInviteNotificationObject gameInviteNotificationObject)
        {
        }

        public void ConnectUser(string userName)
        {
            var newPlayer = new GamifyGamePlayer(userName);

            this.gameController.Connect(newPlayer);

            this.SendPlayerConnectedNotification(newPlayer.UserName);
        }

        public void OnReceive(string userName, string message)
        {
            var gameRequest = JsonConvert.DeserializeObject<GameRequest>(message);
            var gameRequestType = (GameRequestType)gameRequest.Type;

            switch (gameRequestType)
            {
                case GameRequestType.PlayerConnect:
                    throw new ApplicationException("Player Connect message is not supported right now.");
                case GameRequestType.CreateGame:
                    this.CreateGame(gameRequest);
                    break;
                case GameRequestType.GameAccepted:
                    this.GameAccepted(gameRequest);
                    break;
                case GameRequestType.GameMove:
                    this.HandleGameMove(gameRequest.SerializedRequestObject);
                    break;
                case GameRequestType.AbandonGame:
                    this.AbandonGame(gameRequest);
                    break;
                case GameRequestType.PlayerDisconnect:
                    this.DisconnectPlayer(gameRequest);
                    break;
            }
        }

        public void OnDisconnect(string userName)
        {
            this.DisconnectPlayer(userName);
        }

        public void OnError(string userName, Exception ex)
        {
            this.SendErrorNotification(userName, ex);
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

            this.DisconnectPlayer(playerDisconnectObject.PlayerName);
        }

        private void DisconnectPlayer(string userName)
        {
            this.gameController.Disconnect(userName);

            this.SendPlayerDisconnectedNotification(userName);
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

            this.SendNotification(GameNotificationType.GameInvite, gameInviteNotificationObject, newSession.Player2.Information.UserName);
        }

        private void SendGameCreatedNotification(IGameSession newSession)
        {
            var notification = new GameCreatedNotificationObject
            {
                SessionId = newSession.Id,
                Player1Name = newSession.Player1.Information.UserName,
                Player2Name = newSession.Player2.Information.UserName
            };

            this.SendBroadcastNotification(GameNotificationType.GameCreated, notification, newSession.Player1.Information.UserName, newSession.Player2.Information.UserName);
        }

        private void SendAbandonGameNotification(AbandonGameRequestObject abandonGameObject, IGameSession currentSession)
        {
            var notification = new GameAbandonedNotificationObject
            {
                SessionId = abandonGameObject.SessionId,
                PlayerName = abandonGameObject.PlayerName
            };

            this.SendBroadcastNotification(GameNotificationType.GameAbandoned, notification, currentSession.Player1.Information.UserName, currentSession.Player2.Information.UserName);
        }

        private void SendPlayerDisconnectedNotification(string userName)
        {
            var notification = new PlayerDisconnectedNotificationObject
            {
                PlayerName = userName
            };

            this.SendBroadcastNotification(GameNotificationType.PlayerDisconnected, notification);
        }

        private void SendErrorNotification(string userName, Exception exception)
        {
            var notification = new ErrorNotificationObject
            {
                Message = exception.Message
            };

            this.SendBroadcastNotification(GameNotificationType.Error, notification, userName);
        }

        protected void SendBroadcastNotification(GameNotificationType gameNotificationType, object notificationObject)
        {
            this.SendBroadcastNotification(gameNotificationType, notificationObject, this.gameController.Players.Select(p => p.UserName).ToArray());
        }

        protected void SendBroadcastNotification(GameNotificationType gameNotificationType, object notificationObject, params string[] userNames)
        {
            foreach (var userName in userNames)
            {
                this.SendNotification(gameNotificationType, notificationObject, userName);
            }
        }

        protected void SendNotification(GameNotificationType gameNotificationType, object notificationObject, string userName)
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
                sendMessageHandler(this, new GamifyMessageEventArgs(userName, serializedNotification));
            }
        }
    }
}
