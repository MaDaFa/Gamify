namespace Gamify.Sdk.Contracts
{
	public interface IGameControllerService
	{
		void Connect (string playerName);

		void StartGame (string playerName);

		void StartGame (string playerName, string versusPlayerName);

		void Disconnect (string playerName);

		GameMoveResponse SendMove (string senderPlayerName, GameMoveRequest moveRequest);
	}
}
