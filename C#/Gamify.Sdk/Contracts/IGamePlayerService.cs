namespace Gamify.Sdk.Contracts
{
	public interface IGamePlayerService
	{
		GameMoveResponse ReceiveMove (string senderPlayerName, GameMoveRequest moveRequest);
	}
}
