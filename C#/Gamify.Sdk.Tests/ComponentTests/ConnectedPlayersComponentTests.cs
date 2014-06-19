using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Gamify.Sdk.UnitTests.ComponentTests
{
    [TestClass]
    public class ConnectedPlayersComponentTests
    {
        private readonly string requestPlayer = "player1";

        private ISerializer serializer;
        private IList<IGamePlayer> players;
        private Mock<IPlayerService> playerServiceMock;
        private Mock<INotificationService> notificationServiceMock;
        private IGameComponent connectedPlayersComponent;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();

            var sessionHistoryService = Mock.Of<ISessionHistoryService<TestMoveObject, TestResponseObject>>();

            var player1 = new GamePlayer
            {
                DisplayName = "Player 1",
                Name = "player1"
            };
            var player2 = new GamePlayer
            {
                DisplayName = "Player 2",
                Name = "player2"
            };
            var player3 = new GamePlayer
            {
                DisplayName = "Player 3",
                Name = "player3"
            };
            var player4 = new GamePlayer
            {
                DisplayName = "Player 4",
                Name = "player4"
            };

            this.players = new List<IGamePlayer>();
            this.players.Add(player2);
            this.players.Add(player3);
            this.players.Add(player4);

            this.playerServiceMock = new Mock<IPlayerService>();
            this.playerServiceMock
                .Setup(s => s.GetAllConnected(It.Is<string>(x => x == this.requestPlayer)))
                .Returns(this.players);

            this.notificationServiceMock = new Mock<INotificationService>();

            this.connectedPlayersComponent = new ConnectedPlayersComponent(playerServiceMock.Object, notificationServiceMock.Object, this.serializer);
        }

        [TestMethod]
        public void When_HandleGetConnectedPlayers_Then_Success()
        {
            var getConnectedPlayersRequest = new GetConnectedPlayersRequestObject
            {
                PlayerName = this.requestPlayer,
                PageSize = 10
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.GetConnectedPlayers,
                SerializedRequestObject = this.serializer.Serialize(getConnectedPlayersRequest)
            };

            var canHandle = this.connectedPlayersComponent.CanHandleRequest(gameRequest);

            this.connectedPlayersComponent.HandleRequest(gameRequest);

            this.playerServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<GameNotificationType>(t => t == GameNotificationType.SendConnectedPlayers),
                It.Is<object>(o => (((SendConnectedPlayersNotificationObject)o).ConectedPlayersCount == 3)
                    && ((SendConnectedPlayersNotificationObject)o).PlayerName == this.requestPlayer),
                It.Is<string>(x => x == this.requestPlayer)));

            Assert.IsTrue(canHandle);
        }
    }
}
