using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gamify.Sdk.UnitTests.ServiceTests
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
        public void When_ConnectPlayer_Then_Success()
        {
            this.playerService.Connect("player1", "Player 1");
            this.playerService.Connect("player2", "Player 2");

            var players = this.playerService.GetAllConnected();

            Assert.IsNotNull(players);
            Assert.AreEqual(2, players.Count());
        }

        [TestMethod]
        public void When_DisconnectPlayer_Then_Success()
        {
            this.playerService.Connect("player1", "Player 1");
            this.playerService.Connect("player2", "Player 2");

            this.playerService.Disconnect("player1");

            var players = this.playerService.GetAllConnected();

            Assert.IsNotNull(players);
            Assert.AreEqual(1, players.Count());
        }

        [TestMethod]
        public void When_GetRandomWithoutUsers_Then_ReturnNull()
        {
            this.playerService.Connect("player1", "Player 1");

            var existingPlayer = this.playerService.GetRandom(playerNameToExclude: "player1");

            Assert.IsNull(existingPlayer);
        }

        [TestMethod]
        public void When_GetRandomWithUsers_Then_Sucess()
        {
            this.playerService.Connect("player1", "Player 1");
            this.playerService.Connect("player2", "Player 2");

            var existingPlayer = this.playerService.GetRandom(playerNameToExclude: "player1");

            Assert.IsNotNull(existingPlayer);
            Assert.AreEqual("player2", existingPlayer.Name);
            Assert.AreEqual("Player 2", existingPlayer.DisplayName);
        }

        [TestMethod]
        public void When_GetByName_Then_Sucess()
        {
            this.playerService.Connect("player1", "Player 1");
            this.playerService.Connect("player2", "Player 2");
            this.playerService.Connect("player3", "Player 3");

            var player2 = this.playerService.GetByName("player2");

            Assert.IsNotNull(player2);
            Assert.AreEqual("player2", player2.Name);
            Assert.AreEqual("Player 2", player2.DisplayName);
            Assert.IsTrue(player2.IsConnected);
        }

        [TestMethod]
        public void When_GetAllConnectedWithoutExcluding_Then_Sucess()
        {
            this.playerService.Connect("player1", "Player 1");
            this.playerService.Connect("player2", "Player 2");
            this.playerService.Connect("player3", "Player 3");
            this.playerService.Connect("player4", "Player 4");

            this.playerService.Disconnect("player1");
            this.playerService.Disconnect("player3");

            var players = this.playerService.GetAllConnected();

            Assert.IsNotNull(players);
            Assert.AreEqual(2, players.Count());
        }

        [TestMethod]
        public void When_GetAllConnectedExcluding_Then_Sucess()
        {
            this.playerService.Connect("player1", "Player 1");
            this.playerService.Connect("player2", "Player 2");
            this.playerService.Connect("player3", "Player 3");
            this.playerService.Connect("player4", "Player 4");

            this.playerService.Disconnect("player1");
            this.playerService.Disconnect("player3");

            var players = this.playerService.GetAllConnected(playerNameToExclude: "player4");

            Assert.IsNotNull(players);
            Assert.AreEqual(1, players.Count());
        }

        [TestMethod]
        public void When_GetAllWithoutExcluding_Then_Sucess()
        {
            this.playerService.Connect("player1", "Player 1");
            this.playerService.Connect("player2", "Player 2");
            this.playerService.Connect("player3", "Player 3");
            this.playerService.Connect("player4", "Player 4");

            this.playerService.Disconnect("player1");
            this.playerService.Disconnect("player3");

            var players = this.playerService.GetAll();

            Assert.IsNotNull(players);
            Assert.AreEqual(4, players.Count());
        }

        [TestMethod]
        public void When_GetAllExcluding_Then_Sucess()
        {
            this.playerService.Connect("player1", "Player 1");
            this.playerService.Connect("player2", "Player 2");
            this.playerService.Connect("player3", "Player 3");
            this.playerService.Connect("player4", "Player 4");

            this.playerService.Disconnect("player1");
            this.playerService.Disconnect("player3");

            var players = this.playerService.GetAll(playerNameToExclude: "player1");

            Assert.IsNotNull(players);
            Assert.AreEqual(3, players.Count());
        }
    }
}
