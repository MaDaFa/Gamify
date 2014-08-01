using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Contracts.ServerMessages;
using System;
using ThinkUp.Client.SignalR;

namespace Gamify.Client.SignalR.Managers
{
	public interface IGameSelectionManager
	{
		event EventHandler<ServerMessageEventArgs<GameInformationReceivedServerMessage>> GameInformationNotificationReceived;
		
		event EventHandler<ServerMessageEventArgs<ActiveGamesListServerMessage>> ActiveGamesNotificationReceived;
		
		event EventHandler<ServerMessageEventArgs<PendingGamesListServerMessage>> PendingGamesNotificationReceived;
		
		event EventHandler<ServerMessageEventArgs<FinishedGamesListServerMessage>> FinishedGamesNotificationReceived;

		void OpenGame(OpenGameClientMessage openGameClientMessage);

		void RequestActiveGames(GetActiveGamesClientMessage getActiveGamesClientMessage);

		void RequestPendingGames(GetPendingGamesClientMessage getPendingGamesClientMessage);

		void RequestFinishedGames(GetFinishedGamesClientMessage getFinishedGamesClientMessage);
	}
}
