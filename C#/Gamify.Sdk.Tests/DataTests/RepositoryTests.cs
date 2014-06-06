using Gamify.Sdk.Data;
using Gamify.Sdk.Data.Configuration;
using Gamify.Sdk.Data.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Gamify.Sdk.Tests.DataTests
{
    [TestClass]
    public abstract class RepositoryTests<T> where T : MongoEntity
    {
        protected IRepository<T> testRepository;

        [TestInitialize]
        public void Initialize()
        {
            var configuration = GameDataSection.GetConfiguration();

            this.testRepository = new Repository<T>(configuration);
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.testRepository.DeleteAll();
        }
    }
}
