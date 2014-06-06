using Gamify.Sdk.Tests.TestModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Gamify.Sdk.Tests.DataTests
{
    [TestClass]
    public class TestRepositoryTests : RepositoryTests<TestMongoEntity>
    {
        [TestMethod]
        public void When_CreateTestEntity_Then_Success()
        {
            var testEntityName = "test1";
            var testEntity = new TestMongoEntity
            {
                Name = testEntityName,
                DisplayName = "Test 1",
                IsValid = true
            };

            this.testRepository.Create(testEntity);

            var createdTestEntity = this.testRepository.Get(e => e.Name == testEntityName);

            Assert.IsNotNull(createdTestEntity);
            Assert.AreEqual(testEntity.Id, createdTestEntity.Id);
            Assert.AreEqual("Test 1", createdTestEntity.DisplayName);
            Assert.AreEqual(true, createdTestEntity.IsValid);
        }

        [TestMethod]
        public void When_UpdateTestEntity_Then_Success()
        {
            var testEntityName = "test1";
            var testEntity = new TestMongoEntity
            {
                Name = testEntityName,
                DisplayName = "Test 1",
                IsValid = true
            };

            this.testRepository.Create(testEntity);

            var createdTestEntity = this.testRepository.Get(e => e.Name == testEntityName);

            createdTestEntity.IsValid = false;
            createdTestEntity.DisplayName = "Updated Test 1";

            this.testRepository.Update(createdTestEntity);

            var updatedTestEntity = this.testRepository.Get(e => e.Name == testEntityName);

            Assert.IsNotNull(updatedTestEntity);
            Assert.AreEqual(testEntity.Id, updatedTestEntity.Id);
            Assert.AreEqual("Updated Test 1", updatedTestEntity.DisplayName);
            Assert.AreEqual(false, updatedTestEntity.IsValid);
        }

        [TestMethod]
        public void When_DeleteTestEntity_Then_Success()
        {
            var testEntityName = "test1";
            var testEntity = new TestMongoEntity
            {
                Name = testEntityName,
                DisplayName = "Test 1",
                IsValid = true
            };

            this.testRepository.Create(testEntity);

            var createdTestEntity = this.testRepository.Get(e => e.Name == testEntityName);

            this.testRepository.Delete(createdTestEntity);

            var deletedTestEntity = this.testRepository.Get(e => e.Name == testEntityName);

            Assert.IsNull(deletedTestEntity);
        }

        [TestMethod]
        public void When_DeleteTestEntityById_Then_Success()
        {
            var testEntityName = "test1";
            var testEntity = new TestMongoEntity
            {
                Name = testEntityName,
                DisplayName = "Test 1",
                IsValid = true
            };

            this.testRepository.Create(testEntity);

            var createdTestEntity = this.testRepository.Get(e => e.Name == testEntityName);

            this.testRepository.Delete(createdTestEntity.Id);

            var deletedTestEntity = this.testRepository.Get(e => e.Name == testEntityName);

            Assert.IsNull(deletedTestEntity);
        }

        [TestMethod]
        public void When_DeleteAllTestEntities_Then_Success()
        {
            var testEntityName1 = "test1";
            var testEntity1 = new TestMongoEntity
            {
                Name = testEntityName1,
                DisplayName = "Test 1",
                IsValid = true
            };
            var testEntityName2 = "test2";
            var testEntity2 = new TestMongoEntity
            {
                Name = testEntityName2,
                DisplayName = "Test 2",
                IsValid = false
            };
            var testEntityName3 = "test3";
            var testEntity3 = new TestMongoEntity
            {
                Name = testEntityName3,
                DisplayName = "Test 3",
                IsValid = true
            };

            this.testRepository.Create(testEntity1);
            this.testRepository.Create(testEntity2);
            this.testRepository.Create(testEntity3);

            var currentEntitiesCount = this.testRepository.GetAll().Count();

            this.testRepository.DeleteAll();

            var existingEntities = this.testRepository.GetAll();

            Assert.AreEqual(3, currentEntitiesCount);
            Assert.AreEqual(0, existingEntities.Count());
        }

        [TestMethod]
        public void When_GetTestEntitiesWithPredicate_Then_Success()
        {
            var testEntityName1 = "test1";
            var testEntity1 = new TestMongoEntity
            {
                Name = testEntityName1,
                DisplayName = "Test 1",
                IsValid = true
            };
            var testEntityName2 = "test2";
            var testEntity2 = new TestMongoEntity
            {
                Name = testEntityName2,
                DisplayName = "Test 2",
                IsValid = false
            };
            var testEntityName3 = "test3";
            var testEntity3 = new TestMongoEntity
            {
                Name = testEntityName3,
                DisplayName = "Test 3",
                IsValid = true
            };
            var testEntityName4 = "entity4";
            var testEntity4 = new TestMongoEntity
            {
                Name = testEntityName4,
                DisplayName = "Entity 4",
                IsValid = true
            };

            this.testRepository.Create(testEntity1);
            this.testRepository.Create(testEntity2);
            this.testRepository.Create(testEntity3);
            this.testRepository.Create(testEntity4);

            var validEntities = this.testRepository.GetAll(e => e.IsValid);
            var testEntities = this.testRepository.GetAll(e => e.Name.StartsWith("test"));
            var entitiesWith4 = this.testRepository.GetAll(e => e.DisplayName.EndsWith("4"));

            Assert.AreEqual(3, validEntities.Count());
            Assert.AreEqual(3, testEntities.Count());
            Assert.AreEqual(1, entitiesWith4.Count());
        }
    }
}
