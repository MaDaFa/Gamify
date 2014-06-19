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
    public class PendingGamesComponentTests
    {
        private readonly string requestPlayer = "player1";

        private ISerializer serializer;
        private IList<IGameSession> sessions;
        private Mock<ISessionService> sessionServiceMock;
        private Mock<INotificationService> notificationServiceMock;
        private IGameComponent pendingGamesComponent;

        [TestInitialize]
        public void Initialize()
        {
            this.serializer = new JsonSerializer();

            var player1s1 = new TestSessionPlayer()
            {
                SessionName = "player1-vs-player2",
                PendingToMove = false,
                Information = new GamePlayer
                {
                    DisplayName = "Player 1",
                    Name = "player1"
                }
            };
            var player2s1 = new TestSessionPlayer()
            {
                SessionName = "player1-vs-player2",
                PendingToMove = false,
                Information = new GamePlayer
                {
                    DisplayName = "Player 2",
                    Name = "player2"
                }
            };
            var player1s2 = new TestSessionPlayer()
            {
                SessionName = "player1-vs-player3",
                PendingToMove = false,
                Information = new GamePlayer
                {
                    DisplayName = "Player 1",
                    Name = "player1"
                }
            };
            var player3s2 = new TestSessionPlayer()
            {
                SessionName = "player1-vs-player3",
                PendingToMove = false,
                Information = new GamePlayer
                {
                    DisplayName = "Player 3",
                    Name = "player3"
                }
            };

            var session1 = new GameSession(player1s1, player2s1);
            var session2 = new GameSession(player1s2, player3s2);

            this.sessions = new List<IGameSession>();
            this.sessions.Add(session1);
            this.sessions.Add(session2);

            this.sessionServiceMock = new Mock<ISessionService>();
            this.sessionServiceMock
                .Setup(s => s.GetPendings(It.Is<string>(x => x == this.requestPlayer)))
                .Returns(this.sessions)
                .Verifiable();

            this.notificationServiceMock = new Mock<INotificationService>();

            this.pendingGamesComponent = new PendingGamesComponent(sessionServiceMock.Object, notificationServiceMock.Object, this.serializer);
        }

        [TestMethod]
        public void When_HandleGetPendingGames_Then_Success()
        {
            var getPendingGamesRequest = new GetPendingGamesRequestObject
            {
                PlayerName = this.requestPlayer
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.GetPendingGames,
                SerializedRequestObject = this.serializer.Serialize(getPendingGamesRequest)
            };

            var canHandle = this.pendingGamesComponent.CanHandleRequest(gameRequest);

            this.pendingGamesComponent.HandleRequest(gameRequest);

            this.sessionServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<GameNotificationType>(t => t == GameNotificationType.SendPendingGames),
                It.Is<object>(o => (((SendPendingGamesNotificationObject)o).PendingGamesCount == 2)
                    && ((SendPendingGamesNotificationObject)o).PlayerName == this.requestPlayer),
                It.Is<string>(x => x == this.requestPlayer)));

            Assert.IsTrue(canHandle);
        }
    }
}
