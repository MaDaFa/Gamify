using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;
using Gamify.Sdk.Setup.Dependencies;

namespace Gamify.Sdk.Setup
{
    public class GameInitializer : IGameInitializer
    {
        private readonly IGameDefinition gameDefinition;
        private readonly IGameDependencyModule gameDependencyModule;

        public GameInitializer(IGameDefinition gameDefinition)
        {
            this.gameDefinition = gameDefinition;

            var gameDependencyModuleBuilder = new GameDependencyModuleBuilder(this.gameDefinition);

            gameDependencyModuleBuilder.SetDefaults();

            this.gameDependencyModule = gameDependencyModuleBuilder.Build();
        }

        public IGameService Initialize()
        {
            var gameBuilder = this.gameDependencyModule.Get<IGameBuilder>();

            gameBuilder.SetComponents(this.gameDefinition);

            var gameService = gameBuilder.Build();

            return gameService;
        }
    }
}
