using Gamify.Sdk.Setup;
using Gamify.Sdk.Setup.Definition;
using Gamify.Sdk.UnitTests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using ThinkUp.Sdk.Plugins;
using ThinkUp.Sdk.Setup;

namespace Gamify.Sdk.UnitTests.SetupTests
{
    [TestClass]
    public class GamifyConfiguratorTests
    {
        private IGameDefinition<TestMoveObject, TestResponseObject> gameDefinition;

        [TestInitialize]
        public void Initialize()
        {
            var testSessionPlayerFactory = Mock.Of<ISessionPlayerFactory>();
            var moveProcessor = Mock.Of<IMoveProcessor<TestMoveObject, TestResponseObject>>();
            var sessionPlayerSetup = Mock.Of<ISessionPlayerSetup>();
            var gameInviteDecorator = Mock.Of<IGameInviteDecorator>();
            var moveFactory = Mock.Of<IMoveFactory<TestMoveObject>>();
            var moveResultNotificationFactory = Mock.Of<IMoveResultNotificationFactory>();
            var playerHistoryItemFactory = Mock.Of<IPlayerHistoryItemFactory<TestMoveObject, TestResponseObject>>();
            var gameDefinitionMock = new Mock<GameDefinition<TestMoveObject, TestResponseObject>>();

            gameDefinitionMock
                .Setup(d => d.GetSessionPlayerFactory())
                .Returns(testSessionPlayerFactory)
                .Verifiable();
            gameDefinitionMock
                .Setup(d => d.GetMoveProcessor())
                .Returns(moveProcessor)
                .Verifiable();
            gameDefinitionMock
                .Setup(d => d.GetSessionPlayerSetup())
                .Returns(sessionPlayerSetup)
                .Verifiable();
            gameDefinitionMock
                .Setup(d => d.GetGameInviteDecorator())
                .Returns(gameInviteDecorator)
                .Verifiable();
            gameDefinitionMock
                .Setup(d => d.GetMoveFactory())
                .Returns(moveFactory)
                .Verifiable();
            gameDefinitionMock
                .Setup(d => d.GetMoveResultNotificationFactory())
                .Returns(moveResultNotificationFactory)
                .Verifiable();
            gameDefinitionMock
                .Setup(d => d.GetPlayerHistoryItemfactory())
                .Returns(playerHistoryItemFactory)
                .Verifiable();

            this.gameDefinition = gameDefinitionMock.Object;
        }

        [TestMethod]
        public void When_AddGamifyConfiguratorToSetupManager_Then_Success()
        {
            var gamifyConfigurator = new GamifyConfigurator<TestMoveObject, TestResponseObject>(this.gameDefinition);
            var setupManager = new SetupManager();

            setupManager.AddConfigurator(gamifyConfigurator);

            var plugin = setupManager.GetPlugin() as IPluginSetup;

            Assert.IsNotNull(plugin);
            Assert.AreEqual(5, plugin.Components.Count());
        }
    }
}
