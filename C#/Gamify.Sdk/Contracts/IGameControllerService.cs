using System.ServiceModel;

namespace Gamify.Sdk.Contracts
{
	[ServiceContract(CallbackContract = typeof(IGamePlayerService))]
	public interface IGameControllerService
	{
		[OperationContract(IsOneWay = true)]
		void Connect (string playerName);

		[OperationContract(IsOneWay = true)]
		void StartGame (string playerName);

		[OperationContract(IsOneWay = true)]
		void StartGame (string playerName, string versusPlayerName);

		[OperationContract(IsOneWay = true)]
		void Disconnect (string playerName);

		[OperationContract(IsOneWay = true)]
		GameMoveResponse SendMove (string senderPlayerName, GameMoveRequest moveRequest);
	}
}
