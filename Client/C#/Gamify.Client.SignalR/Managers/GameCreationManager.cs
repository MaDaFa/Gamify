using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Contracts.ServerMessages;
using System;
using ThinkUp.Client.SignalR;
using ThinkUp.Client.SignalR.Client;
using ThinkUp.Client.SignalR.Services;

namespace Gamify.Client.SignalR.Managers
{
	public class GameCreationManager : IGameCreationManager
	{
		private readonly IPluginService<CreateGameClientMessage, GameInviteReceivedServerMessage> createGameService;
		private readonly IPluginService<AcceptGameClientMessage, GameCreatedServerMessage> acceptGameService;
		private readonly IPluginService<RejectGameClientMessage, GameRejectedServerMessage> rejectGameService;

		public event EventHandler<ServerMessageEventArgs<GameInviteReceivedServerMessage>> GameInviteNotificationReceived;
		public event EventHandler<ServerMessageEventArgs<GameCreatedServerMessage>> GameCreatedNotificationReceived;
		public event EventHandler<ServerMessageEventArgs<GameRejectedServerMessage>> GameRejectedNotificationReceived;

		public GameCreationManager(IPluginClient pluginClient, ISerializer serializer)
		{
			this.createGameService = new PluginService<CreateGameClientMessage, GameInviteReceivedServerMessage>(GamifyClientMessageType.CreateGame, GamifyServerMessageType.GameInviteReceived, pluginClient, serializer);
			this.acceptGameService = new PluginService<AcceptGameClientMessage, GameCreatedServerMessage>(GamifyClientMessageType.AcceptGame, GamifyServerMessageType.GameCreated, pluginClient, serializer);
			this.rejectGameService = new PluginService<RejectGameClientMessage, GameRejectedServerMessage>(GamifyClientMessageType.RejectGame, GamifyServerMessageType.GameRejected, pluginClient, serializer);

			this.createGameService.NotificationReceived += (sender, args) =>
			{
				if (this.GameInviteNotificationReceived != null)
				{
					this.GameInviteNotificationReceived(this, args);
				}
			};

			this.acceptGameService.NotificationReceived += (sender, args) =>
			{
				if (this.GameCreatedNotificationReceived != null)
				{
					this.GameCreatedNotificationReceived(this, args);
				}
			};

			this.rejectGameService.NotificationReceived += (sender, args) =>
			{
				if (this.GameRejectedNotificationReceived != null)
				{
					this.GameRejectedNotificationReceived(this, args);
				}
			};
		}

		public void CreateGame(CreateGameClientMessage createGameClientMessage)
		{
			this.createGameService.Send(createGameClientMessage);
		}

		public void AcceptGame(AcceptGameClientMessage acceptGameClientMessage)
		{
			this.acceptGameService.Send(acceptGameClientMessage);
		}

		public void RejectGame(RejectGameClientMessage rejectGameClientMessage)
		{
			this.rejectGameService.Send(rejectGameClientMessage);
		}
	}
}
