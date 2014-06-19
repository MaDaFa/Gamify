using Gamify.Sdk.Data.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gamify.Sdk.UnitTests.DataTests
{
    [TestClass]
    public class GamePlayerRepositoryTests : RepositoryTests<GamePlayer>
    {
        [TestMethod]
        public void When_CreateGamePlayer_Then_Success()
        {
            var playerName = GetUniqueName("player");
            var player = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = playerName
            };

            this.testRepository.Create(player);

            var createdPlayer = this.testRepository.Get(e => e.Name == playerName);

            Assert.IsNotNull(createdPlayer);
            Assert.AreEqual(player.Id, createdPlayer.Id);
            Assert.AreEqual(player.DisplayName, createdPlayer.DisplayName);
            Assert.AreEqual(playerName, createdPlayer.Name);
        }

        [TestMethod]
        public void When_UpdateGamePlayer_Then_Success()
        {
            var playerName = GetUniqueName("player");
            var player = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = playerName
            };

            this.testRepository.Create(player);

            var createdPlayer = this.testRepository.Get(e => e.Name == playerName);

            createdPlayer.DisplayName = GetUniqueName("Updated Player");

            this.testRepository.Update(createdPlayer);

            var updatedPlayer = this.testRepository.Get(e => e.Name == playerName);

            Assert.IsNotNull(updatedPlayer);
            Assert.AreEqual(player.Id, updatedPlayer.Id);
            Assert.AreEqual(createdPlayer.DisplayName, updatedPlayer.DisplayName);
        }

        [TestMethod]
        public void When_DeleteGamePlayer_Then_Success()
        {
            var playerName = GetUniqueName("player");
            var player = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = playerName
            };

            this.testRepository.Create(player);

            var createdPlayer = this.testRepository.Get(e => e.Name == playerName);

            this.testRepository.Delete(createdPlayer);

            var deletedPlayer = this.testRepository.Get(e => e.Name == playerName);

            Assert.IsNull(deletedPlayer);
        }

        [TestMethod]
        public void When_DeleteAllPlayers_Then_Success()
        {
            var player1Name = GetUniqueName("player");
            var player1 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = player1Name
            };
            var player2Name = GetUniqueName("player");
            var player2 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = player2Name
            };
            var player3Name = GetUniqueName("player");
            var player3 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = player3Name
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
            var player1Name = GetUniqueName("player");
            var player1 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = player1Name
            };
            var player2Name = GetUniqueName("player");
            var player2 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = player2Name
            };
            var player3Name = GetUniqueName("player");
            var player3 = new GamePlayer
            {
                DisplayName = GetUniqueName("Player"),
                Name = player3Name
            };
            var player4Name = GetUniqueName("test player");
            var player4 = new GamePlayer
            {
                DisplayName = GetUniqueName("Test Player"),
                Name = player4Name
            };

            this.testRepository.Create(player1);
            this.testRepository.Create(player2);
            this.testRepository.Create(player3);
            this.testRepository.Create(player4);

            var testPlayers = this.testRepository.GetAll(e => e.DisplayName.Contains("Test"));

            Assert.AreEqual(1, testPlayers.Count());
        }
    }
}
