using System;
using Gamify.Sdk.Interfaces;

namespace Gamify.Core
{
	public class GameSession : ISingleGameSession
	{
		public bool IsReady
		{
			get
			{
				return this.Player1 != null && this.Player2 != null;
			}
		}

		public IGamePlayer Player1 { get; private set; }

		public IGamePlayer Player2 { get; private set; }

		public GameSession ()
		{
		}

		public GameSession (IGamePlayer player1)
		{
			this.AddPlayer (player1);
		}

		public GameSession (IGamePlayer player1, IGamePlayer player2)
		{
			this.AddPlayer (player1);
			this.AddPlayer (player2);
		}

		public void AddPlayer (IGamePlayer player)
		{
			if(this.IsReady)
			{
				throw new ApplicationException ("The game session is full");
			}

			if(this.Player1 == null)
			{
				this.Player1 = player;
			}
			else
			{
				this.Player2 = player;
			}
		}

		public void RemovePlayer (string playerName)
		{
			if(this.Player1 != null && this.Player1.Name == playerName)
			{
				this.Player1 = null;
			}
			else if(this.Player2 != null && this.Player2.Name == playerName)
			{
				this.Player2 = null;
			}
			else 
			{
				var message = string.Format ("Player {0} is not part of this session", playerName);

				throw new ApplicationException (message);
			}
		}

		public bool HasPlayer (string playerName)
		{
			return (this.Player1 != null && this.Player1.Name == playerName) || (this.Player2 != null && this.Player2.Name == playerName);
		}
	}
}
