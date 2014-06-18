using Gamify.Sdk.Data;
using Gamify.Sdk.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Sdk.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IRepository<GamePlayer> playerRepository;

        public PlayerService(IRepository<GamePlayer> playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public IEnumerable<IGamePlayer> GetAll(string playerNameToExclude = null)
        {
            return this.playerRepository.GetAll(p => p.Name != playerNameToExclude);
        }

        public IEnumerable<IGamePlayer> GetAllConnected(string playerNameToExclude = null)
        {
            return this.playerRepository.GetAll(p => p.IsConnected && p.Name != playerNameToExclude);
        }

        public IGamePlayer GetByName(string playerName)
        {
            return this.playerRepository.Get(p => p.Name == playerName);
        }

        public IGamePlayer GetRandom(string playerNameToExclude = null)
        {
            return this.GetAll(playerNameToExclude)
                .OrderBy(p => Guid.NewGuid())
                .FirstOrDefault();
        }

        public bool Exist(string playerName)
        {
            return this.playerRepository.Exist(p => p.Name == playerName);
        }

        ///<exception cref="GameServiceException">GameServiceException</exception>
        public void Connect(string playerName, string name = null)
        {
            var existingPlayer = this.playerRepository.Get(p => p.Name == playerName);

            try
            {
                if (existingPlayer == null)
                {
                    var newPlayer = new GamePlayer
                    {
                        Name = playerName,
                        DisplayName = name ?? playerName,
                        IsConnected = true
                    };

                    this.playerRepository.Create(newPlayer);
                }
                else
                {
                    existingPlayer.IsConnected = true;

                    this.playerRepository.Update(existingPlayer);
                }
            }
            catch (GameDataException gameDataEx)
            {
                var actionKeyword = existingPlayer == null ? "creating" : "updating";
                var errorMessage = string.Format("An error occured when {0} the player {1} to connect", actionKeyword, playerName);

                throw new GameServiceException(errorMessage, gameDataEx);
            }
        }

        ///<exception cref="GameServiceException">GameServiceException</exception>
        public void Disconnect(string playerName)
        {
            var existingPlayer = this.playerRepository.Get(p => p.Name == playerName);

            if (existingPlayer == null)
            {
                var errorMessage = string.Format("The player to disconnect, {0}, does not exist", playerName);

                throw new GameServiceException(errorMessage);
            }

            existingPlayer.IsConnected = false;

            try
            {
                this.playerRepository.Update(existingPlayer);
            }
            catch (GameDataException gameDataEx)
            {
                var errorMessage = string.Format("An error occured when trying to disconnect the player {0}", playerName);

                throw new GameServiceException(errorMessage, gameDataEx);
            }
        }
    }
}
