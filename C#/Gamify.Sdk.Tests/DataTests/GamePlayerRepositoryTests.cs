using Gamify.Sdk.Data.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gamify.Sdk.Tests.DataTests
{
    [TestClass]
    public class GamePlayerRepositoryTests : RepositoryTests<GamePlayer>
    {
        [TestMethod]
        public void When_CreateGamePlayer_Then_Success()
        {
            var playerName = "player1";
            var player = new GamePlayer
            {
                Name = "Player 1",
                UserName = playerName
            };

            this.testRepository.Create(player);

            var createdPlayer = this.testRepository.Get(e => e.UserName == playerName);

            Assert.IsNotNull(createdPlayer);
            Assert.AreEqual(player.Id, createdPlayer.Id);
            Assert.AreEqual("Player 1", createdPlayer.Name);
            Assert.AreEqual(playerName, createdPlayer.UserName);
        }

        [TestMethod]
        public void When_UpdateGamePlayer_Then_Success()
        {
            var playerName = "player1";
            var player = new GamePlayer
            {
                Name = "Player 1",
                UserName = playerName
            };

            this.testRepository.Create(player);

            var createdPlayer = this.testRepository.Get(e => e.UserName == playerName);

            createdPlayer.Name = "Updated Player 1";

            this.testRepository.Update(createdPlayer);

            var updatedPlayer = this.testRepository.Get(e => e.UserName == playerName);

            Assert.IsNotNull(updatedPlayer);
            Assert.AreEqual(player.Id, updatedPlayer.Id);
            Assert.AreEqual("Updated Player 1", updatedPlayer.Name);
        }

        [TestMethod]
        public void When_DeleteGamePlayer_Then_Success()
        {
            var playerName = "player1";
            var player = new GamePlayer
            {
                Name = "Player 1",
                UserName = playerName
            };

            this.testRepository.Create(player);

            var createdPlayer = this.testRepository.Get(e => e.UserName == playerName);

            this.testRepository.Delete(createdPlayer);

            var deletedPlayer = this.testRepository.Get(e => e.UserName == playerName);

            Assert.IsNull(deletedPlayer);
        }

        [TestMethod]
        public void When_DeleteGamePlayerById_Then_Success()
        {
            var playerName = "player1";
            var player = new GamePlayer
            {
                Name = "Player 1",
                UserName = playerName
            };

            this.testRepository.Create(player);

            var createdPlayer = this.testRepository.Get(e => e.UserName == playerName);

            this.testRepository.Delete(createdPlayer.Id);

            var deletedPlayer = this.testRepository.Get(e => e.UserName == playerName);

            Assert.IsNull(deletedPlayer);
        }

        [TestMethod]
        public void When_DeleteAllPlayers_Then_Success()
        {
            var player1Name = "player1";
            var player1 = new GamePlayer
            {
                Name = "Player 1",
                UserName = player1Name
            };
            var player2Name = "player2";
            var player2 = new GamePlayer
            {
                Name = "Player 2",
                UserName = player2Name
            };
            var player3Name = "player3";
            var player3 = new GamePlayer
            {
                Name = "Player 3",
                UserName = player3Name
            };

            this.testRepository.Create(player1);
            this.testRepository.Create(player2);
            this.testRepository.Create(player3);

            var currentPlayersCount = this.testRepository.GetAll().Count();

            this.testRepository.DeleteAll();

            var existingEntities = this.testRepository.GetAll();

            Assert.AreEqual(3, currentPlayersCount);
            Assert.AreEqual(0, existingEntities.Count());
        }

        [TestMethod]
        public void When_GetGamePlayersWithPredicate_Then_Success()
        {
            var player1Name = "player1";
            var player1 = new GamePlayer
            {
                Name = "Player 1",
                UserName = player1Name
            };
            var player2Name = "player2";
            var player2 = new GamePlayer
            {
                Name = "Player 2",
                UserName = player2Name
            };
            var player3Name = "player3";
            var player3 = new GamePlayer
            {
                Name = "Player 3",
                UserName = player3Name
            };
            var player4Name = "test player";
            var player4 = new GamePlayer
            {
                Name = "Test Player",
                UserName = player4Name
            };

            this.testRepository.Create(player1);
            this.testRepository.Create(player2);
            this.testRepository.Create(player3);
            this.testRepository.Create(player4);

            var testPlayers = this.testRepository.GetAll(e => e.Name.Contains("Test"));

            Assert.AreEqual(1, testPlayers.Count());
        }
    }
}
