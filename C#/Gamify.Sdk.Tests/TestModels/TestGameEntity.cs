using Gamify.Sdk.Data.Entities;

namespace Gamify.Sdk.UnitTests.TestModels
{
    public class TestGameEntity : GameEntity
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsValid { get; set; }
    }
}
