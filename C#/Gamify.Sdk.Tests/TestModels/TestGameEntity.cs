using Gamify.Sdk.Data.Entities;

namespace Gamify.Sdk.Tests.TestModels
{
    public class TestGameEntity : GameEntity
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsValid { get; set; }
    }
}
