using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Contracts.ServerMessages;
using System;
using ThinkUp.Client.SignalR;
using ThinkUp.Client.SignalR.Client;
using ThinkUp.Client.SignalR.Services;

namespace Gamify.Client.SignalR.Managers
{
	public class GameProgressManager<T> where T : IMoveResultReceivedServerMessage
	{
		private readonly IPluginService<SendMoveClientMessage, T> gameMoveService;
		private readonly IServerListener<MoveReceivedServerMessage> gameMoveListener;
		private readonly IPluginService<AbandonGameClientMessage, GameAbandonedServerMessage> abandonGameService;
		private readonly IServerListener<GameFinishedServerMessage> gameFinishedListener;

		public event EventHandler<ServerMessageEventArgs<T>> GameMoveResultNotificationReceived;
		public event EventHandler<ServerMessageEventArgs<MoveReceivedServerMessage>> GameMoveNotificationReceived;
		public event EventHandler<ServerMessageEventArgs<GameAbandonedServerMessage>> GameAbandonedNotificationReceived;
		public event EventHandler<ServerMessageEventArgs<GameFinishedServerMessage>> GameFinishedNotificationReceived;

		public GameProgressManager(IPluginClient pluginClient, ISerializer serializer)
		{
			this.gameMoveService = new PluginService<SendMoveClientMessage, T>(GamifyClientMessageType.SendMove, GamifyServerMessageType.MoveResultReceived, pluginClient, serializer);
			this.gameMoveListener = new ServerListener<MoveReceivedServerMessage>(GamifyServerMessageType.MoveReceived, pluginClient, serializer);
			this.abandonGameService = new PluginService<AbandonGameClientMessage, GameAbandonedServerMessage>(GamifyClientMessageType.AbandonGame, GamifyServerMessageType.GameAbandoned, pluginClient, serializer);
			this.gameFinishedListener = new ServerListener<GameFinishedServerMessage>(GamifyServerMessageType.GameFinished, pluginClient, serializer);

			this.gameMoveService.NotificationReceived += (sender, args) =>
			{
				if (this.GameMoveResultNotificationReceived != null)
				{
					this.GameMoveResultNotificationReceived(this, args);
				}
			};

			this.gameMoveListener.NotificationReceived += (sender, args) =>
			{
				if (this.GameMoveNotificationReceived != null)
				{
					this.GameMoveNotificationReceived(this, args);
				}
			};

			this.abandonGameService.NotificationReceived += (sender, args) =>
			{
				if (this.GameAbandonedNotificationReceived != null)
				{
					this.GameAbandonedNotificationReceived(this, args);
				}
			};

			this.gameFinishedListener.NotificationReceived += (sender, args) =>
			{
				if (this.GameFinishedNotificationReceived != null)
				{
					this.GameFinishedNotificationReceived(this, args);
				}
			};
		}

		public void SendMove(SendMoveClientMessage sendMoveClientMessage)
		{
			this.gameMoveService.Send(sendMoveClientMessage);
		}

		public void AbandonGame(AbandonGameClientMessage abandonGameClientMessage)
		{
			this.abandonGameService.Send(abandonGameClientMessage);
		}
	}
}
