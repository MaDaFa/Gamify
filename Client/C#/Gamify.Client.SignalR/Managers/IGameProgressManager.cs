using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Contracts.ServerMessages;
using System;
using ThinkUp.Client.SignalR;

namespace Gamify.Client.SignalR.Managers
{
	public interface IGameProgressManager<T> where T : IMoveResultReceivedServerMessage
	{
		event EventHandler<ServerMessageEventArgs<T>> GameMoveResultNotificationReceived;

		event EventHandler<ServerMessageEventArgs<MoveReceivedServerMessage>> GameMoveNotificationReceived;
		
		event EventHandler<ServerMessageEventArgs<GameAbandonedServerMessage>> GameAbandonedNotificationReceived;
		
		event EventHandler<ServerMessageEventArgs<GameFinishedServerMessage>> GameFinishedNotificationReceived;

		void SendMove(SendMoveClientMessage sendMoveClientMessage);

		void AbandonGame(AbandonGameClientMessage abandonGameClientMessage);
	}
}
