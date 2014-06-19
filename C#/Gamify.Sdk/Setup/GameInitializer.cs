using Gamify.Sdk.Data;
using Gamify.Sdk.Data.Configuration;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;
using Gamify.Sdk.Setup.Dependencies;

namespace Gamify.Sdk.Setup
{
    public class GameInitializer : IGameInitializer
    {
        ///<exception cref="GameSetupException">GameSetupException</exception>
        public IGameService Initialize<TMove, UResponse>(IGameDefinition<TMove, UResponse> gameDefinition)
        {
            var gameDependencyModuleBuilder = new GameDependencyModuleBuilder();
            var gameConfiguration = GameDataSection.Instance() as GameDataSection;

            gameDependencyModuleBuilder.SetDependency<IGameDataSection, GameDataSection>(gameConfiguration);
            gameDependencyModuleBuilder.SetOpenGenericDependency(typeof(IRepository<>), typeof(Repository<>));
            gameDependencyModuleBuilder.SetDependency<ISerializer, JsonSerializer>();
            gameDependencyModuleBuilder.SetDependency<INotificationService, NotificationService>();
            gameDependencyModuleBuilder.SetDependency<IPlayerService, PlayerService>();
            gameDependencyModuleBuilder.SetDependency<ISessionHistoryService<TMove, UResponse>, SessionHistoryService<TMove, UResponse>>();
            gameDependencyModuleBuilder.SetDependency<ISessionPlayerFactory>(gameDefinition.GetSessionPlayerFactory());
            gameDependencyModuleBuilder.SetDependency<ISessionService, SessionService>();
            gameDependencyModuleBuilder.SetDependency<IMoveProcessor<TMove, UResponse>>(gameDefinition.GetMoveProcessor());
            gameDependencyModuleBuilder.SetDependency<IMoveService<TMove, UResponse>, MoveService<TMove, UResponse>>();
            gameDependencyModuleBuilder.SetDependency<IGameBuilder<TMove, UResponse>, GameBuilder<TMove, UResponse>>();

            var gameDependencyModule = gameDependencyModuleBuilder.Build();
            var gameBuilder = gameDependencyModule.Get<IGameBuilder<TMove, UResponse>>();

            gameBuilder.SetComponents(gameDefinition);

            var gameService = gameBuilder.Build();

            return gameService;
        }
    }
}
