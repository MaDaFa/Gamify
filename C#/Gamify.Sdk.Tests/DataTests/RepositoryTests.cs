using Gamify.Sdk.Data;
using Gamify.Sdk.Data.Configuration;
using Gamify.Sdk.Data.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Gamify.Sdk.Tests.DataTests
{
    [TestClass]
    public abstract class RepositoryTests<T> where T : GameEntity
    {
        private readonly bool cleanDbWhenFinishes;

        protected IRepository<T> testRepository;

        protected RepositoryTests(bool cleanDbWhenFinishes = true)
        {
            this.cleanDbWhenFinishes = cleanDbWhenFinishes;
        }

        [TestInitialize]
        public void Initialize()
        {
            var configuration = GameDataSection.GetConfiguration();

            this.testRepository = new Repository<T>(configuration);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (this.cleanDbWhenFinishes)
            {
                this.testRepository.DeleteAll();
            }
        }

        protected static string GetUniqueName(string name = null)
        {
            var uniqueName = Guid.NewGuid().ToString();

            return string.IsNullOrEmpty(name) ? uniqueName : string.Concat(name, "-", uniqueName);
        }
    }
}
