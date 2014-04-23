using Gamify.Sdk.Interfaces;

namespace Gamify.Sdk.Contracts
{
	public class GameMoveRequest
	{
		private object gameMoveRequest;

		public IGameMove<T> GetMove<T>()
		{
			return this.gameMoveRequest as IGameMove<T>;
		}

		public static GameMoveRequest Create<T>(IGameMove<T> genericMove)
		{
			var moveRequest = new GameMoveRequest ();

			moveRequest.gameMoveRequest = genericMove;

			return moveRequest;
		}
	}
}
