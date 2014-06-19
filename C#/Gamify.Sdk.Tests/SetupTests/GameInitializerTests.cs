using Gamify.Sdk.Setup;
using Gamify.Sdk.Setup.Definition;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gamify.Sdk.UnitTests.SetupTests
{
    [TestClass]
    public class GameInitializerTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void When_InitializeGame_Then_Success()
        {
            var moveProcessor = Mock.Of<IMoveProcessor<TestMoveObject, TestResponseObject>>();
            var testSessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();
            var gameDefinitionMock = new Mock<GameDefinition<TestMoveObject, TestResponseObject>>();

            gameDefinitionMock
                .Setup(d => d.GetSessionPlayerFactory())
                .Returns(testSessionPlayerFactory)
                .Verifiable();
            gameDefinitionMock
                .Setup(d => d.GetMoveProcessor())
                .Returns(moveProcessor)
                .Verifiable();

            var gameInitializer = new GameInitializer();
            var gameService1 = gameInitializer.Initialize(gameDefinitionMock.Object);
            var gameService2 = gameInitializer.Initialize(gameDefinitionMock.Object);

            gameDefinitionMock.VerifyAll();

            Assert.IsNotNull(gameService1);
            Assert.IsNotNull(gameService2);
            Assert.AreNotEqual(gameService1, gameService2);
        }
    }
}
