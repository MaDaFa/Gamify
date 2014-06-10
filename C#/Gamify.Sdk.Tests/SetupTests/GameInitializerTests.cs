using Gamify.Sdk.Setup;
using Gamify.Sdk.Setup.Definition;
using Gamify.Sdk.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gamify.Sdk.Tests.SetupTests
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
            var testSessionPlayerFactory = Mock.Of<ISessionPlayerFactory<TestMoveObject, TestResponseObject>>();
            var gameDefinitionMock = new Mock<GameDefinition<TestMoveObject, TestResponseObject>>();

            gameDefinitionMock
                .Setup(d => d.GetSessionPlayerFactory())
                .Returns(testSessionPlayerFactory)
                .Verifiable();

            var gameInitializer = new GameInitializer(gameDefinitionMock.Object);
            var gameBuilder1 = gameInitializer.Initialize();
            var gameBuilder2 = gameInitializer.Initialize();

            Assert.IsNotNull(gameBuilder1);
            Assert.IsNotNull(gameBuilder2);
            Assert.AreNotEqual(gameBuilder1, gameBuilder2);
        }
    }
}
