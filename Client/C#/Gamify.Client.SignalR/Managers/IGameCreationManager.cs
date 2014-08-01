using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Contracts.ServerMessages;
using System;
using ThinkUp.Client.SignalR;

namespace Gamify.Client.SignalR.Managers
{
	public interface IGameCreationManager
	{
		event EventHandler<ServerMessageEventArgs<GameInviteReceivedServerMessage>> GameInviteNotificationReceived;
		
		event EventHandler<ServerMessageEventArgs<GameCreatedServerMessage>> GameCreatedNotificationReceived;
		
		event EventHandler<ServerMessageEventArgs<GameRejectedServerMessage>> GameRejectedNotificationReceived;

		void CreateGame(CreateGameClientMessage createGameClientMessage);

		void AcceptGame(AcceptGameClientMessage acceptGameClientMessage);

		void RejectGame(RejectGameClientMessage rejectGameClientMessage);
	}
}
