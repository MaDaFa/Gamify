using Gamify.Sdk.Data;
using Gamify.Sdk.Data.Configuration;
using Gamify.Sdk.Data.Entities;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;
using Gamify.Sdk.Setup.Definition;
using Gamify.Sdk.Setup.Dependencies;
using Gamify.Sdk.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Gamify.Sdk.Tests.SetupTests
{
    [TestClass]
    public class GameDependencyModuleTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void When_BuildDependencies_Then_Success()
        {
            var testSessionPlayerFactory = Mock.Of<ISessionPlayerFactory<TestMoveObject, TestResponseObject>>();
            var gameDefinitionMock = new Mock<GameDefinition<TestMoveObject, TestResponseObject>>();

            gameDefinitionMock
                .Setup(d => d.GetSessionPlayerFactory())
                .Returns(testSessionPlayerFactory)
                .Verifiable();

            var gameDependencyModuleBuilder = new GameDependencyModuleBuilder(gameDefinitionMock.Object);

            gameDependencyModuleBuilder.SetDefaults();
            
            var gameDependencyModule = gameDependencyModuleBuilder.Build();

            var gameConfiguration = gameDependencyModule.Get<IGameDataSection>();
            var gamePlayerRepository = gameDependencyModule.Get<IRepository<GamePlayer>>();
            var gameSessionRepository = gameDependencyModule.Get<IRepository<GameSession>>();
            var sessionHistoryRepository = gameDependencyModule.Get<IRepository<SessionHistory<TestMoveObject, TestResponseObject>>>();
            var serializer = gameDependencyModule.Get<ISerializer>();
            var notificationService = gameDependencyModule.Get<INotificationService>();
            var playerService = gameDependencyModule.Get<IPlayerService>();
            var sessionHistoryService = gameDependencyModule.Get<ISessionHistoryService>();
            var typedSessionHistoryService = gameDependencyModule.Get<ISessionHistoryService<TestMoveObject, TestResponseObject>>();
            var sessionPlayerFactory = gameDependencyModule.Get<ISessionPlayerFactory>();
            var sessionService = gameDependencyModule.Get<ISessionService>();
            var moveService = gameDependencyModule.Get<IMoveService>();
            var gameBuilder = gameDependencyModule.Get<IGameBuilder>();

            gameDefinitionMock.VerifyAll();

            Assert.IsNotNull(gameConfiguration);
            Assert.IsNotNull(gamePlayerRepository);
            Assert.IsNotNull(gameSessionRepository);
            Assert.IsNotNull(sessionHistoryRepository);
            Assert.IsNotNull(serializer);
            Assert.IsTrue(serializer is JsonSerializer);
            Assert.IsNotNull(notificationService);
            Assert.IsNotNull(playerService);
            Assert.IsNotNull(sessionHistoryService);
            Assert.IsNotNull(typedSessionHistoryService);
            Assert.IsNotNull(sessionPlayerFactory);
            Assert.AreEqual(testSessionPlayerFactory, sessionPlayerFactory);
            Assert.IsNotNull(sessionService);
            Assert.IsNotNull(moveService);
            Assert.IsNotNull(gameBuilder);
        }
    }
}
