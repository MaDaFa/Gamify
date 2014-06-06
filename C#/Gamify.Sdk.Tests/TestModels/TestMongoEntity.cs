using Gamify.Sdk.Data.Entities;

namespace Gamify.Sdk.Tests.TestModels
{
    public class TestMongoEntity : MongoEntity
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsValid { get; set; }
    }
}
