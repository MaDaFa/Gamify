namespace Gamify.Sdk.Contracts
{
	public interface IGamePlayerService
	{
		GameMoveResponse ReceiveMove<T> (string senderPlayerName, GameMoveRequest moveRequest);
	}
}
