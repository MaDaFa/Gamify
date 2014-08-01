using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Contracts.ServerMessages;
using System;
using ThinkUp.Client.SignalR;
using ThinkUp.Client.SignalR.Client;
using ThinkUp.Client.SignalR.Services;

namespace Gamify.Client.SignalR.Managers
{
	public class GameSelectionManager : IGameSelectionManager
	{
		private readonly IPluginService<OpenGameClientMessage, GameInformationReceivedServerMessage> openGameService;
		private readonly IPluginService<GetActiveGamesClientMessage, ActiveGamesListServerMessage> activeGamesService;
        private readonly IPluginService<GetPendingGamesClientMessage, PendingGamesListServerMessage> pendingGamesService;
        private readonly IPluginService<GetFinishedGamesClientMessage, FinishedGamesListServerMessage> finishedGamesService;

		public event EventHandler<ServerMessageEventArgs<GameInformationReceivedServerMessage>> GameInformationNotificationReceived;
        public event EventHandler<ServerMessageEventArgs<ActiveGamesListServerMessage>> ActiveGamesNotificationReceived;
        public event EventHandler<ServerMessageEventArgs<PendingGamesListServerMessage>> PendingGamesNotificationReceived;
        public event EventHandler<ServerMessageEventArgs<FinishedGamesListServerMessage>> FinishedGamesNotificationReceived;

		public GameSelectionManager(IPluginClient pluginClient, ISerializer serializer)
        {
			this.openGameService = new PluginService<OpenGameClientMessage, GameInformationReceivedServerMessage>(GamifyClientMessageType.OpenGame, GamifyServerMessageType.GameInformationReceived, pluginClient, serializer);
			this.activeGamesService = new PluginService<GetActiveGamesClientMessage, ActiveGamesListServerMessage>(GamifyClientMessageType.GetActiveGames, GamifyServerMessageType.ActiveGamesList, pluginClient, serializer);
			this.pendingGamesService = new PluginService<GetPendingGamesClientMessage, PendingGamesListServerMessage>(GamifyClientMessageType.GetPendingGames, GamifyServerMessageType.PendingGamesList, pluginClient, serializer);
			this.finishedGamesService = new PluginService<GetFinishedGamesClientMessage, FinishedGamesListServerMessage>(GamifyClientMessageType.GetFinishedGames, GamifyServerMessageType.FinishedGamesList, pluginClient, serializer);

			this.openGameService.NotificationReceived += (sender, args) =>
			{
				if (this.GameInformationNotificationReceived != null)
				{
					this.GameInformationNotificationReceived(this, args);
				}
			};

            this.activeGamesService.NotificationReceived += (sender, args) =>
            {
				if (this.ActiveGamesNotificationReceived != null)
				{
					this.ActiveGamesNotificationReceived(this, args);
				}
            };

            this.pendingGamesService.NotificationReceived += (sender, args) =>
            {
				if (this.PendingGamesNotificationReceived != null)
				{
					this.PendingGamesNotificationReceived(this, args);
				}
            };

            this.finishedGamesService.NotificationReceived += (sender, args) =>
            {
				if (this.FinishedGamesNotificationReceived != null)
				{
					this.FinishedGamesNotificationReceived(this, args);
				}
            };
        }

		public void OpenGame(OpenGameClientMessage openGameClientMessage)
		{
			this.openGameService.Send(openGameClientMessage);
		}

		public void RequestActiveGames(GetActiveGamesClientMessage getActiveGamesClientMessage)
        {
			this.activeGamesService.Send(getActiveGamesClientMessage);
        }

		public void RequestPendingGames(GetPendingGamesClientMessage getPendingGamesClientMessage)
        {
			this.pendingGamesService.Send(getPendingGamesClientMessage);
        }

		public void RequestFinishedGames(GetFinishedGamesClientMessage getFinishedGamesClientMessage)
        {
			this.finishedGamesService.Send(getFinishedGamesClientMessage);
        }
	}
}
