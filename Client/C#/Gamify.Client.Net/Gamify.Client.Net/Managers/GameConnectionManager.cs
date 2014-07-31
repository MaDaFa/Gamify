using Gamify.Client.Net.Client;
using Gamify.Client.Net.Contracts.Notifications;
using Gamify.Client.Net.Contracts.Requests;
using Gamify.Client.Net.Services;
using System;

namespace Gamify.Client.Net.Managers
{
    public class GameConnectionManager
    {
        private readonly IGameService<PlayerConnectRequestObject, PlayerConnectedNotificationObject> playerConnectService;
        private readonly IGameService<PlayerDisconnectRequestObject, PlayerDisconnectedNotificationObject> playerDisconnectService;
        private readonly IGameService<GetConnectedPlayersRequestObject, SendConnectedPlayersNotificationObject> connectedPlayersService;

        public event EventHandler<GameNotificationEventArgs<PlayerConnectedNotificationObject>> PlayerConnectedNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<PlayerDisconnectedNotificationObject>> PlayerDisconnectedNotificationReceived;
        public event EventHandler<GameNotificationEventArgs<SendConnectedPlayersNotificationObject>> ConnectedPlayersNotificationReceived;

        public GameConnectionManager(IGameClientFactory clientFactory)
        {
            var gameClient = clientFactory.Create();

            this.playerConnectService = new GameService<PlayerConnectRequestObject, PlayerConnectedNotificationObject>(GameRequestType.PlayerConnect, GameNotificationType.PlayerConnected, gameClient);
            this.playerDisconnectService = new GameService<PlayerDisconnectRequestObject, PlayerDisconnectedNotificationObject>(GameRequestType.PlayerDisconnect, GameNotificationType.PlayerDisconnected, gameClient);
            this.connectedPlayersService = new GameService<GetConnectedPlayersRequestObject, SendConnectedPlayersNotificationObject>(GameRequestType.GetConnectedPlayers, GameNotificationType.SendConnectedPlayers, gameClient);

            this.playerConnectService.NotificationReceived += (sender, args) =>
            {
                this.NotifyPlayerConnected(args);
            };

            this.playerDisconnectService.NotificationReceived += (sender, args) =>
            {
                this.NotifyPlayerDisconnected(args);
            };

            this.connectedPlayersService.NotificationReceived += (sender, args) =>
            {
                this.NotifyConnectedPlayers(args);
            };
        }

        public void ConnectPlayer(PlayerConnectRequestObject playerConnectRequest)
        {
            this.playerConnectService.Send(playerConnectRequest);
        }

        public void DisconnectPlayer(PlayerDisconnectRequestObject playerDisconnectRequest)
        {
            this.playerDisconnectService.Send(playerDisconnectRequest);
        }

        public void GetConnectedPlayers(GetConnectedPlayersRequestObject getConnectedPlayersRequest)
        {
            this.connectedPlayersService.Send(getConnectedPlayersRequest);
        }

        private void NotifyPlayerConnected(GameNotificationEventArgs<PlayerConnectedNotificationObject> args)
        {
            if (this.PlayerConnectedNotificationReceived != null)
            {
                this.PlayerConnectedNotificationReceived(this, args);
            }
        }

        private void NotifyPlayerDisconnected(GameNotificationEventArgs<PlayerDisconnectedNotificationObject> args)
        {
            if (this.PlayerDisconnectedNotificationReceived != null)
            {
                this.PlayerDisconnectedNotificationReceived(this, args);
            }
        }

        private void NotifyConnectedPlayers(GameNotificationEventArgs<SendConnectedPlayersNotificationObject> args)
        {
            if (this.ConnectedPlayersNotificationReceived != null)
            {
                this.ConnectedPlayersNotificationReceived(this, args);
            }
        }
    }
}
