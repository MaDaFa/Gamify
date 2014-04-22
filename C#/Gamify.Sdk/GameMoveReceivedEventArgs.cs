using System;
using Gamify.Sdk.Interfaces;

namespace Gamify.Sdk
{
	public class GameMoveReceivedEventArgs : EventArgs
	{
		public IGameMove<T> GetMove<T> ()
		{
			throw new NotImplementedException ();
		}
	}
}
