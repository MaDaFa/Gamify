using Gamify.Sdk.PluginComponents;
using Gamify.Sdk.Services;
using Gamify.Sdk.Setup.Definition;
using ThinkUp.Sdk.Plugins.PluginComponents;
using ThinkUp.Sdk.Setup;
using ThinkUp.Sdk.Setup.Dependencies;

namespace Gamify.Sdk.Setup
{
    public class GamifyConfigurator<TMove, UResponse> : IConfigurator
    {
        private readonly IGameDefinition<TMove, UResponse> gameDefinition;

        public GamifyConfigurator(IGameDefinition<TMove, UResponse> gameDefinition)
        {
            this.gameDefinition = gameDefinition;
        }

        public void ConfigureDependencies(IDependencyContainerBuilder dependencyContainerBuilder)
        {
            dependencyContainerBuilder.SetDependency<ISessionHistoryService<TMove, UResponse>, SessionHistoryService<TMove, UResponse>>();
            dependencyContainerBuilder.SetDependency<ISessionPlayerFactory>(gameDefinition.GetSessionPlayerFactory());
            dependencyContainerBuilder.SetDependency<ISessionService, SessionService>();
            dependencyContainerBuilder.SetDependency<IMoveProcessor<TMove, UResponse>>(gameDefinition.GetMoveProcessor());
            dependencyContainerBuilder.SetDependency<IMoveService<TMove, UResponse>, MoveService<TMove, UResponse>>();
            dependencyContainerBuilder.SetDependency<ISessionPlayerSetup>(gameDefinition.GetSessionPlayerSetup());
            dependencyContainerBuilder.SetDependency<IGameInviteDecorator>(gameDefinition.GetGameInviteDecorator());
            dependencyContainerBuilder.SetDependency<IPluginComponent, GameCreationPluginComponent>();
            dependencyContainerBuilder.SetDependency<IMoveFactory<TMove>>(gameDefinition.GetMoveFactory());
            dependencyContainerBuilder.SetDependency<IMoveResultNotificationFactory>(gameDefinition.GetMoveResultNotificationFactory());
            dependencyContainerBuilder.SetDependency<IPluginComponent, GameProgressPluginComponent<TMove, UResponse>>();
            dependencyContainerBuilder.SetDependency<IPlayerHistoryItemFactory<TMove, UResponse>>(gameDefinition.GetPlayerHistoryItemfactory());
            dependencyContainerBuilder.SetDependency<IPluginComponent, GameSelectionPluginComponent<TMove, UResponse>>();
        }
    }
}
