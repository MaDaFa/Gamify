using System.ServiceModel;

namespace Gamify.Sdk.Contracts
{
	public interface IGamePlayerService
	{
		[OperationContract(IsOneWay = true)]
		GameMoveResponse ReceiveMove<T> (string senderPlayerName, GameMoveRequest moveRequest);
	}
}
