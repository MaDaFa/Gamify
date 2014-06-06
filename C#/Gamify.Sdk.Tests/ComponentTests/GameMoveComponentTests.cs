using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;
using Gamify.Sdk.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gamify.Sdk.Tests.ComponentTests
{
    [TestClass]
    public class GameMoveComponentTests
    {
        private readonly string sessionName = "player1-vs-player2";
        private readonly string requestPlayer = "player2";

        private ISerializer serializer;
        private IGameSession session;
        private Mock<IMoveService> moveServiceMock;
        private Mock<ISessionService> sessionServiceMock;
        private Mock<INotificationService> notificationServiceMock;
        private Mock<IMoveHandler> moveHandlerMock;
        private Mock<IMoveResultNotificationFactory> moveResultNotificationFactoryMock;
        private IMoveResultNotificationObject moveResultNotificationObject;

        [TestInitialize]
        public void Initialize()
        {
        }

        private IGameComponent GetMoveComponent(bool toWin = false)
        {
            this.serializer = new JsonSerializer();

            var sessionHistoryService = Mock.Of<ISessionHistoryService<TestMoveObject, TestResponseObject>>();

            var player1 = new TestSessionPlayer(sessionHistoryService)
            {
                SessionName = this.sessionName,
                PendingToMove = false,
                Information = new GamePlayer
                {
                    Name = "Player 1",
                    UserName = "player1"
                }
            };
            var player2 = new TestSessionPlayer(sessionHistoryService)
            {
                SessionName = this.sessionName,
                PendingToMove = false,
                Information = new GamePlayer
                {
                    Name = "Player 2",
                    UserName = "player2"
                }
            };

            this.session = new GameSession(player1, player2);

            this.moveServiceMock = new Mock<IMoveService>();

            this.sessionServiceMock = new Mock<ISessionService>();
            this.sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == this.session.Name)))
                .Returns(this.session)
                .Verifiable();

            this.notificationServiceMock = new Mock<INotificationService>();

            var moveResponse = new TestResponse(new TestResponseObject { IsCorrect = true }) { IsWin = toWin };

            this.moveHandlerMock = new Mock<IMoveHandler>();
            this.moveHandlerMock
                .Setup(h => h.Handle(It.Is<MoveRequestObject>(o => o.SessionName == this.sessionName
                    && o.PlayerName == this.requestPlayer), It.Is<IMoveService>(s => s == this.moveServiceMock.Object)))
                .Returns(moveResponse)
                .Verifiable();

            this.moveResultNotificationObject = Mock.Of<IMoveResultNotificationObject>(o => o.SessionName == this.sessionName && o.PlayerName == this.requestPlayer);
            this.moveResultNotificationFactoryMock = new Mock<IMoveResultNotificationFactory>();

            if (!toWin)
            {
                this.moveResultNotificationFactoryMock
                    .Setup(f => f.Create(It.Is<MoveRequestObject>(o => o.SessionName == this.sessionName
                    && o.PlayerName == this.requestPlayer), It.Is<IGameMoveResponse>(r => r == moveResponse)))
                    .Returns(this.moveResultNotificationObject);
            }

            return new GameMoveComponent(this.moveServiceMock.Object, this.sessionServiceMock.Object, this.notificationServiceMock.Object,
                this.moveHandlerMock.Object, this.moveResultNotificationFactoryMock.Object, this.serializer);
        }

        [TestMethod]
        public void When_HandleGameMove_Then_Success()
        {
            var moveComponent = this.GetMoveComponent(toWin: false);

            var gameMoveRequest = new MoveRequestObject
            {
                SessionName = this.sessionName,
                PlayerName = this.requestPlayer,
                MoveInformation = "Test"
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.GameMove,
                SerializedRequestObject = this.serializer.Serialize(gameMoveRequest)
            };

            var canHandle = moveComponent.CanHandleRequest(gameRequest);

            moveComponent.HandleRequest(gameRequest);

            this.sessionServiceMock.VerifyAll();
            this.moveHandlerMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<GameNotificationType>(t => t == GameNotificationType.GameMove),
                    It.Is<object>(o => ((MoveNotificationObject)o).SessionName == this.session.Name &&
                    ((MoveNotificationObject)o).PlayerName == this.requestPlayer),
                    It.Is<string>(x => x == this.session.Player1.Information.UserName)));
            this.notificationServiceMock.Verify(s => s.Send(It.Is<GameNotificationType>(t => t == GameNotificationType.GameMoveResult),
                    It.Is<object>(o => ((IMoveResultNotificationObject)o).SessionName == this.session.Name &&
                    ((IMoveResultNotificationObject)o).PlayerName == this.requestPlayer),
                    It.Is<string>(x => x == this.requestPlayer)));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void When_HandleWinnerGameMove_Then_Success()
        {
            var moveComponent = this.GetMoveComponent(toWin: true);

            var gameMoveRequest = new MoveRequestObject
            {
                SessionName = this.sessionName,
                PlayerName = this.requestPlayer,
                MoveInformation = "Test"
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.GameMove,
                SerializedRequestObject = this.serializer.Serialize(gameMoveRequest)
            };

            var canHandle = moveComponent.CanHandleRequest(gameRequest);

            moveComponent.HandleRequest(gameRequest);

            this.sessionServiceMock.VerifyAll();
            this.moveHandlerMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<GameNotificationType>(t => t == GameNotificationType.GameFinished),
                    It.Is<object>(o => ((GameFinishedNotificationObject)o).SessionName == this.session.Name &&
                    ((GameFinishedNotificationObject)o).WinnerPlayerName == this.requestPlayer),
                    It.Is<string>(x => x == this.session.Player2.Information.UserName),
                    It.Is<string>(x => x == this.session.Player1.Information.UserName)));

            Assert.IsTrue(canHandle);
        }
    }
}
