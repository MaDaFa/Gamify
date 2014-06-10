using Gamify.Sdk.Components;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup;
using Gamify.Sdk.Setup.Definition;
using Gamify.Sdk.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Gamify.Sdk.Tests.SetupTests
{
    [TestClass]
    public class GameBuilderTests
    {
        private IGameBuilder testGameBuilder;

        [TestInitialize]
        public void Initialize()
        {
            var playerService = Mock.Of<IPlayerService>();
            var sessionService = Mock.Of<ISessionService>();
            var sessionHistoryService = Mock.Of<ISessionHistoryService<TestMoveObject, TestResponseObject>>();
            var moveService = Mock.Of<IMoveService>();
            var notificationService = Mock.Of<INotificationService>();
            var serializer = new JsonSerializer();

            this.testGameBuilder = new GameBuilder(playerService, sessionService,
                sessionHistoryService, moveService, notificationService, serializer);
        }

        [TestMethod]
        public void When_BuildGameWithBasicDefinition_Then_Success()
        {
            var testGameDefinitionMock = new Mock<GameDefinition<TestMoveObject, TestResponseObject>>();

            this.testGameBuilder.SetComponents(testGameDefinitionMock.Object);
            
            var testGameService = this.testGameBuilder.Build();

            Assert.AreEqual(11, testGameService.Components.Count());
        }

        [TestMethod]
        public void When_BuildGameWithCustomComponents_Then_Success()
        {
            var notificationServiceMock = Mock.Of<INotificationService>();
            var testGameDefinitionMock = new Mock<GameDefinition<TestMoveObject, TestResponseObject>>();
            var customComponents = new List<IGameComponent>();

            customComponents.Add(Mock.Of<IGameComponent>(c => c.NotificationService == notificationServiceMock));
            customComponents.Add(Mock.Of<IGameComponent>(c => c.NotificationService == notificationServiceMock));
            customComponents.Add(Mock.Of<IGameComponent>(c => c.NotificationService == notificationServiceMock));
            customComponents.Add(Mock.Of<IGameComponent>(c => c.NotificationService == notificationServiceMock));
            customComponents.Add(Mock.Of<IGameComponent>(c => c.NotificationService == notificationServiceMock));
            customComponents.Add(Mock.Of<IGameComponent>(c => c.NotificationService == notificationServiceMock));

            testGameDefinitionMock
                .Setup(d => d.GetCustomComponents())
                .Returns(customComponents)
                .Verifiable();

            this.testGameBuilder.SetComponents(testGameDefinitionMock.Object);

            var testGameService = this.testGameBuilder.Build();

            testGameDefinitionMock.VerifyAll();

            Assert.AreEqual(17, testGameService.Components.Count());
        }
    }
}
