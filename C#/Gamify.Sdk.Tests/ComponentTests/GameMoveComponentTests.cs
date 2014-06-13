using Gamify.Sdk.Components;
using Gamify.Sdk.Contracts.Notifications;
using Gamify.Sdk.Contracts.Requests;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;
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
        private readonly string moveInformation = "Test";

        private ISerializer serializer;
        private IGameSession session;
        private Mock<IMoveService<TestMoveObject, TestResponseObject>> moveServiceMock;
        private Mock<ISessionService> sessionServiceMock;
        private Mock<ISessionHistoryService<TestMoveObject, TestResponseObject>> sessionHistoryServiceMock;
        private Mock<IMoveFactory<TestMoveObject>> moveFactoryMock;
        private Mock<INotificationService> notificationServiceMock;
        private Mock<IMoveResultNotificationFactory> moveResultNotificationFactoryMock;

        [TestInitialize]
        public void Initialize()
        {
        }

        private IGameComponent GetMoveComponent(bool toWin = false)
        {
            this.serializer = new JsonSerializer();

            var player1 = new TestSessionPlayer()
            {
                SessionName = this.sessionName,
                PendingToMove = false,
                Information = new GamePlayer
                {
                    DisplayName = "Player 1",
                    Name = "player1"
                }
            };
            var player2 = new TestSessionPlayer()
            {
                SessionName = this.sessionName,
                PendingToMove = false,
                Information = new GamePlayer
                {
                    DisplayName = "Player 2",
                    Name = "player2"
                }
            };

            this.session = new GameSession(player1, player2);

            var testMoveObject = new TestMoveObject { Answer = "Test Answer" };
            var testMove = new TestMove(testMoveObject);

            var moveResponse = new TestResponse(new TestResponseObject { IsCorrect = true }) { IsWin = toWin };

            this.moveServiceMock = new Mock<IMoveService<TestMoveObject, TestResponseObject>>();
            this.moveServiceMock
                .Setup(s => s.Handle(It.Is<string>(x => x == this.session.Name), It.Is<string>(x => x == this.requestPlayer),
                    It.Is<IGameMove<TestMoveObject>>(m => m == testMove)))
                .Returns(moveResponse)
                .Verifiable();

            this.sessionServiceMock = new Mock<ISessionService>();
            this.sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == this.session.Name)))
                .Returns(this.session)
                .Verifiable();

            var destinationPlayer = "player1";

            this.sessionHistoryServiceMock = new Mock<ISessionHistoryService<TestMoveObject, TestResponseObject>>();
            this.sessionHistoryServiceMock
                .Setup(x => x.Add(It.Is<string>(s => s == this.sessionName),
                    It.Is<string>(s => s == destinationPlayer),
                    It.Is<ISessionHistoryItem<TestMoveObject, TestResponseObject>>(i => i.Move == testMove.MoveObject 
                        && i.Response == moveResponse.MoveResponseObject)))
                .Verifiable();

            this.notificationServiceMock = new Mock<INotificationService>();

            this.moveFactoryMock = new Mock<IMoveFactory<TestMoveObject>>();
            this.moveFactoryMock
                .Setup(f => f.Create(It.Is<string>(s => s == this.moveInformation)))
                .Returns(testMove)
                .Verifiable();

            var moveResultNotificationObject = Mock.Of<IMoveResultNotificationObject>(o => o.SessionName == this.sessionName && o.PlayerName == this.requestPlayer);
            
            this.moveResultNotificationFactoryMock = new Mock<IMoveResultNotificationFactory>();

            if (!toWin)
            {
                this.moveResultNotificationFactoryMock
                    .Setup(f => f.Create(It.Is<MoveRequestObject>(o => o.SessionName == this.sessionName
                    && o.PlayerName == this.requestPlayer), It.Is<IGameMoveResponse>(r => r == moveResponse)))
                    .Returns(moveResultNotificationObject)
                    .Verifiable();
            }

            return new GameMoveComponent<TestMoveObject, TestResponseObject>(moveServiceMock.Object, this.sessionServiceMock.Object,
                sessionHistoryServiceMock.Object, this.notificationServiceMock.Object, moveFactoryMock.Object, this.moveResultNotificationFactoryMock.Object, this.serializer);
        }

        [TestMethod]
        public void When_HandleGameMove_Then_Success()
        {
            var moveComponent = this.GetMoveComponent(toWin: false);

            var gameMoveRequest = new MoveRequestObject
            {
                SessionName = this.sessionName,
                PlayerName = this.requestPlayer,
                MoveInformation = this.moveInformation
            };
            var gameRequest = new GameRequest
            {
                Type = (int)GameRequestType.GameMove,
                SerializedRequestObject = this.serializer.Serialize(gameMoveRequest)
            };

            var canHandle = moveComponent.CanHandleRequest(gameRequest);

            moveComponent.HandleRequest(gameRequest);

            this.moveServiceMock.VerifyAll();
            this.sessionServiceMock.VerifyAll();
            this.sessionHistoryServiceMock.VerifyAll();
            this.moveFactoryMock.VerifyAll();
            this.moveResultNotificationFactoryMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<GameNotificationType>(t => t == GameNotificationType.GameMove),
                    It.Is<object>(o => ((MoveNotificationObject)o).SessionName == this.session.Name &&
                    ((MoveNotificationObject)o).PlayerName == this.requestPlayer),
                    It.Is<string>(x => x == this.session.Player1Name)));
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

            this.moveServiceMock.VerifyAll();
            this.sessionServiceMock.VerifyAll();
            this.sessionHistoryServiceMock.VerifyAll();
            this.moveFactoryMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<GameNotificationType>(t => t == GameNotificationType.GameFinished),
                    It.Is<object>(o => ((GameFinishedNotificationObject)o).SessionName == this.session.Name &&
                    ((GameFinishedNotificationObject)o).WinnerPlayerName == this.requestPlayer),
                    It.Is<string>(x => x == this.session.Player2Name),
                    It.Is<string>(x => x == this.session.Player1Name)));

            Assert.IsTrue(canHandle);
        }
    }
}
