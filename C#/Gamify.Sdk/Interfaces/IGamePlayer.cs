using System;

namespace Gamify.Sdk.Interfaces
{
	public delegate EventHandler<GameMoveReceivedEventArgs> ReceiveMove(object sender, GameMoveReceivedEventArgs e);

    public interface IGamePlayer
    {
		event EventHandler<GameMoveReceivedEventArgs> ReceiveMove;

		string Name { get; }

		bool IsPlaying { get; }

		void OnReceiveMove (object sender, GameMoveReceivedEventArgs e);
    }
}
