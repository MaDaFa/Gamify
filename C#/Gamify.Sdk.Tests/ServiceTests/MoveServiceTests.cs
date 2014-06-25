using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gamify.Sdk.UnitTests.ServiceTests
{
    [TestClass]
    public class MoveServiceTests
    {
        private IMoveService<TestMoveObject, TestResponseObject> moveService;

        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void UT_When_HandleMove_Then_Success()
        {
            var sessionName = "player1-vs-player2";
            var player1 = new TestSessionPlayer()
            {
                SessionName = sessionName,
                PendingToMove = false,
                Information = new GamePlayer
                {
                    DisplayName = "Player 1",
                    Name = "player1"
                }
            };
            var player2 = new TestSessionPlayer()
            {
                SessionName = sessionName,
                PendingToMove = true,
                Information = new GamePlayer
                {
                    DisplayName = "Player 2",
                    Name = "player2"
                }
            };
            var session = new GameSession(player1, player2);

            var testMoveObject = new TestMoveObject { Answer = "Test Answer" };
            var testMove = new TestMove(testMoveObject);

            var moveResponse = new TestResponse(new TestResponseObject { IsCorrect = true }) { IsWin = false };

            var sessionServiceMock = new Mock<ISessionService>();

            sessionServiceMock
                .Setup(s => s.GetByName(It.Is<string>(x => x == sessionName)))
                .Returns(session)
                .Verifiable();

            var moveProcessorMock = new Mock<IMoveProcessor<TestMoveObject, TestResponseObject>>();

            moveProcessorMock
                .Setup(p => p.Process(It.Is<SessionGamePlayer>(x => x == player2), It.Is<IGameMove<TestMoveObject>>(m => m == testMove)))
                .Returns(moveResponse)
                .Verifiable();

            this.moveService = new MoveService<TestMoveObject, TestResponseObject>(sessionServiceMock.Object, moveProcessorMock.Object);

            var response = this.moveService.Handle(sessionName, player1.Information.Name, testMove);

            sessionServiceMock.VerifyAll();
            moveProcessorMock.VerifyAll();

            Assert.IsNotNull(response);
            Assert.AreEqual(moveResponse, response);
        }
    }
}
