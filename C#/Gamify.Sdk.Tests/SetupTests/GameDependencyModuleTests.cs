using Gamify.Sdk.Data;
using Gamify.Sdk.Data.Configuration;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Dependencies;
using Gamify.Sdk.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var configuration = GameDataSection.Instance();
            var gameDependencyModuleBuilder = new GameDependencyModuleBuilder();

            gameDependencyModuleBuilder.SetDependency<ITestServiceBar, TestServiceBar>();
            gameDependencyModuleBuilder.SetDependency<ITestServiceFoo, TestServiceFoo>();
            gameDependencyModuleBuilder.SetDependency<IGameDataSection>(configuration);
            gameDependencyModuleBuilder.SetOpenGenericDependency(typeof(IRepository<>), typeof(Repository<>));
            gameDependencyModuleBuilder.SetOpenGenericDependency(typeof(ISessionHistoryService<,>), typeof(SessionHistoryService<,>));

            var gameDependencyModule = gameDependencyModuleBuilder.Build();

            var testServiceFoo = gameDependencyModule.Get<ITestServiceFoo>();
            var typedSessionHistoryService = gameDependencyModule.Get<ISessionHistoryService<TestMoveObject, TestResponseObject>>();

            Assert.IsNotNull(testServiceFoo);
            Assert.IsNotNull(testServiceFoo.TestServiceBar);
            Assert.IsNotNull(typedSessionHistoryService);
        }
    }
}
