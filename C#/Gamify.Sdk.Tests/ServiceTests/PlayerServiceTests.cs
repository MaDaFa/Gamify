using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gamify.Sdk.Tests.ServiceTests
{
    [TestClass]
    public class PlayerServiceTests
    {
        private IPlayerService playerService;

        [TestInitialize]
        public void Initialize()
        {
            var playerRepository = new TestRepository<GamePlayer>();

            this.playerService = new PlayerService(playerRepository);
        }

        [TestMethod]
        public void When_CreatePlayer_Then_Success()
        {
            this.playerService.Create("player1", "Player 1");
            this.playerService.Create("player2", "Player 2");

            var players = this.playerService.GetAll();

            Assert.IsNotNull(players);
            Assert.AreEqual(2, players.Count());
        }

        [TestMethod]
        public void When_GetRandomWithoutUsers_Then_ReturnNull()
        {
            this.playerService.Create("player1", "Player 1");

            var existingPlayer = this.playerService.GetRandom(playerNameToExclude: "player1");

            Assert.IsNull(existingPlayer);
        }

        [TestMethod]
        public void When_GetRandomWithUsers_Then_Sucess()
        {
            this.playerService.Create("player1", "Player 1");
            this.playerService.Create("player2", "Player 2");

            var existingPlayer = this.playerService.GetRandom(playerNameToExclude: "player1");

            Assert.IsNotNull(existingPlayer);
            Assert.AreEqual("player2", existingPlayer.UserName);
            Assert.AreEqual("Player 2", existingPlayer.Name);
        }

        [TestMethod]
        public void When_GetByName_Then_Sucess()
        {
            this.playerService.Create("player1", "Player 1");
            this.playerService.Create("player2", "Player 2");
            this.playerService.Create("player3", "Player 3");

            var player2 = this.playerService.GetByName("player2");

            Assert.IsNotNull(player2);
            Assert.AreEqual("player2", player2.UserName);
            Assert.AreEqual("Player 2", player2.Name);
        }

        [TestMethod]
        public void When_GetAllWithoutExcluding_Then_Sucess()
        {
            this.playerService.Create("player1", "Player 1");
            this.playerService.Create("player2", "Player 2");
            this.playerService.Create("player3", "Player 3");

            var players = this.playerService.GetAll();

            Assert.IsNotNull(players);
            Assert.AreEqual(3, players.Count());
        }

        [TestMethod]
        public void When_GetAllExcluding_Then_Sucess()
        {
            this.playerService.Create("player1", "Player 1");
            this.playerService.Create("player2", "Player 2");
            this.playerService.Create("player3", "Player 3");

            var players = this.playerService.GetAll(playerNameToExclude: "player3");

            Assert.IsNotNull(players);
            Assert.AreEqual(2, players.Count());
        }
    }
}
