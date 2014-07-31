using Gamify.Sdk.Contracts.ClientMessages;
using Gamify.Sdk.Contracts.ServerMessages;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Interfaces;
using Gamify.Sdk.PluginComponents;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ThinkUp.Sdk;
using ThinkUp.Sdk.Contracts.ClientMessages;
using ThinkUp.Sdk.Data.Entities;
using ThinkUp.Sdk.Plugins.PluginComponents;
using ThinkUp.Sdk.Services;

namespace Gamify.Sdk.UnitTests.PluginComponentTests
{
    [TestClass]
    public class GameProgressPluginComponentTests
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
            this.serializer = new JsonSerializer();

            var player1 = new TestSessionPlayer()
            {
                SessionName = this.sessionName,
                PendingToMove = false,
                Information = new User
                {
                    DisplayName = "Player 1",
                    Name = "player1"
                }
            };
            var player2 = new TestSessionPlayer()
            {
                SessionName = this.sessionName,
                PendingToMove = true,
                Information = new User
                {
                    DisplayName = "Player 2",
                    Name = "player2"
                }
            };

            this.session = new GameSession(player1, player2);

            this.moveServiceMock = new Mock<IMoveService<TestMoveObject, TestResponseObject>>();
            this.sessionServiceMock = new Mock<ISessionService>();
            this.sessionHistoryServiceMock = new Mock<ISessionHistoryService<TestMoveObject, TestResponseObject>>();
            this.notificationServiceMock = new Mock<INotificationService>();
            this.moveFactoryMock = new Mock<IMoveFactory<TestMoveObject>>();
            this.moveResultNotificationFactoryMock = new Mock<IMoveResultNotificationFactory>();
        }

        [TestMethod]
        public void UT_When_HandleAbandonGame_Then_Success()
        {
            this.sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == this.session.Name)))
                .Returns(this.session)
                .Verifiable();
            this.sessionServiceMock
                .Setup(s => s.Abandon(It.Is<string>(x => x == this.session.Name)))
                .Verifiable();

            var gameProgressPluginComponent = this.GetGameProgressPluginComponent();
            var abandonGameClientMessage = new AbandonGameClientMessage
            {
                SessionName = this.sessionName,
                UserName = this.requestPlayer
            };
            var clientContract = new ClientContract
            {
                Type = GamifyClientMessageType.AbandonGame,
                Sender = this.requestPlayer,
                SerializedClientMessage = this.serializer.Serialize(abandonGameClientMessage)
            };
            var canHandle = gameProgressPluginComponent.CanHandleClientMessage(clientContract);

            gameProgressPluginComponent.HandleClientMessage(clientContract);

            this.sessionServiceMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<int>(t => t == GamifyServerMessageType.GameAbandoned),
                    It.Is<object>(o => ((GameAbandonedServerMessage)o).SessionName == this.session.Name),
                    It.Is<string>(x => x == this.session.Player1Name),
                    It.Is<string>(x => x == this.session.Player2Name)));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void UT_When_HandleGameMove_Then_Success()
        {
            var testMoveObject = new TestMoveObject { Answer = "Test Answer" };
            var testMove = new TestMove(testMoveObject);
            var moveResponse = new TestResponse(new TestResponseObject { IsCorrect = true }) { IsWin = false };
            var moveResultNotificationObject = Mock.Of<IMoveResultReceivedServerMessage>(o => o.SessionName == this.sessionName && o.PlayerName == this.requestPlayer);

            this.sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == this.session.Name)))
                .Returns(this.session)
                .Verifiable();
            this.moveFactoryMock
                .Setup(f => f.Create(It.Is<string>(s => s == this.moveInformation)))
                .Returns(testMove)
                .Verifiable();
            this.moveServiceMock
                .Setup(s => s.Handle(It.Is<string>(x => x == this.session.Name), It.Is<string>(x => x == this.requestPlayer),
                    It.Is<IGameMove<TestMoveObject>>(m => m == testMove)))
                .Returns(moveResponse)
                .Verifiable();
            this.sessionHistoryServiceMock
                .Setup(x => x.Add(It.Is<string>(s => s == this.sessionName),
                    It.Is<string>(s => s == this.requestPlayer),
                    It.Is<ISessionHistoryItem<TestMoveObject, TestResponseObject>>(i => i.Move == testMove.MoveObject
                        && i.Response == moveResponse.MoveResponseObject)))
                .Verifiable();
            this.moveResultNotificationFactoryMock
                    .Setup(f => f.Create(It.Is<SendMoveClientMessage>(o => o.SessionName == this.sessionName
                        && o.UserName == this.requestPlayer), It.Is<IGameMoveResponse>(r => r == moveResponse)))
                    .Returns(moveResultNotificationObject)
                    .Verifiable();

            var gameProgressPluginComponent = this.GetGameProgressPluginComponent();
            var sendMoveClientMessage = new SendMoveClientMessage
            {
                SessionName = this.sessionName,
                UserName = this.requestPlayer,
                MoveInformation = this.moveInformation
            };
            var clientContract = new ClientContract
            {
                Type = GamifyClientMessageType.SendMove,
                Sender = this.requestPlayer,
                SerializedClientMessage = this.serializer.Serialize(sendMoveClientMessage)
            };
            var canHandle = gameProgressPluginComponent.CanHandleClientMessage(clientContract);

            gameProgressPluginComponent.HandleClientMessage(clientContract);

            this.moveServiceMock.VerifyAll();
            this.sessionServiceMock.VerifyAll();
            this.sessionHistoryServiceMock.VerifyAll();
            this.moveFactoryMock.VerifyAll();
            this.moveResultNotificationFactoryMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.Send(It.Is<int>(t => t == GamifyServerMessageType.MoveReceived),
                    It.Is<object>(o => ((MoveReceivedServerMessage)o).SessionName == this.session.Name &&
                    ((MoveReceivedServerMessage)o).PlayerName == this.requestPlayer),
                    It.Is<string>(x => x == this.session.Player1Name)));
            this.notificationServiceMock.Verify(s => s.Send(It.Is<int>(t => t == GamifyServerMessageType.MoveResultReceived),
                    It.Is<object>(o => ((IMoveResultReceivedServerMessage)o).SessionName == this.session.Name &&
                    ((IMoveResultReceivedServerMessage)o).PlayerName == this.requestPlayer),
                    It.Is<string>(x => x == this.requestPlayer)));

            Assert.IsTrue(canHandle);
        }

        [TestMethod]
        public void UT_When_HandleWinnerGameMove_Then_Success()
        {
            var testMoveObject = new TestMoveObject { Answer = "Test Answer" };
            var testMove = new TestMove(testMoveObject);
            var moveResponse = new TestResponse(new TestResponseObject { IsCorrect = true }) { IsWin = true };
            var moveResultNotificationObject = Mock.Of<IMoveResultReceivedServerMessage>(o => o.SessionName == this.sessionName && o.PlayerName == this.requestPlayer);

            this.sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == this.session.Name)))
                .Returns(this.session)
                .Verifiable();
            this.moveFactoryMock
                .Setup(f => f.Create(It.Is<string>(s => s == this.moveInformation)))
                .Returns(testMove)
                .Verifiable();
            this.moveServiceMock
                .Setup(s => s.Handle(It.Is<string>(x => x == this.session.Name), It.Is<string>(x => x == this.requestPlayer),
                    It.Is<IGameMove<TestMoveObject>>(m => m == testMove)))
                .Returns(moveResponse)
                .Verifiable();
            this.sessionHistoryServiceMock
                .Setup(x => x.Add(It.Is<string>(s => s == this.sessionName),
                    It.Is<string>(s => s == this.requestPlayer),
                    It.Is<ISessionHistoryItem<TestMoveObject, TestResponseObject>>(i => i.Move == testMove.MoveObject
                        && i.Response == moveResponse.MoveResponseObject)))
                .Verifiable();
            this.sessionServiceMock
                    .Setup(s => s.Finish(It.Is<string>(x => x == this.sessionName)))
                    .Verifiable();

            var gameProgressPluginComponent = this.GetGameProgressPluginComponent();
            var gameMoveRequest = new SendMoveClientMessage
            {
                SessionName = this.sessionName,
                UserName = this.requestPlayer,
                MoveInformation = "Test"
            };
            var clientContract = new ClientContract
            {
                Type = GamifyClientMessageType.SendMove,
                Sender = this.requestPlayer,
                SerializedClientMessage = this.serializer.Serialize(gameMoveRequest)
            };
            var canHandle = gameProgressPluginComponent.CanHandleClientMessage(clientContract);

            gameProgressPluginComponent.HandleClientMessage(clientContract);

            this.moveServiceMock.VerifyAll();
            this.sessionServiceMock.VerifyAll();
            this.sessionHistoryServiceMock.VerifyAll();
            this.moveFactoryMock.VerifyAll();
            this.notificationServiceMock.Verify(s => s.SendBroadcast(It.Is<int>(t => t == GamifyServerMessageType.GameFinished),
                    It.Is<object>(o => ((GameFinishedServerMessage)o).SessionName == this.session.Name &&
                    ((GameFinishedServerMessage)o).WinnerPlayerName == this.requestPlayer),
                    It.Is<string>(x => x == this.session.Player2Name),
                    It.Is<string>(x => x == this.session.Player1Name)));

            Assert.IsTrue(canHandle);
        }

        private IPluginComponent GetGameProgressPluginComponent()
        {
            return new GameProgressPluginComponent<TestMoveObject, TestResponseObject>(this.moveServiceMock.Object, this.sessionServiceMock.Object, 
                this.sessionHistoryServiceMock.Object, this.notificationServiceMock.Object, this.moveFactoryMock.Object, this.moveResultNotificationFactoryMock.Object, this.serializer);
        }
    }
}
