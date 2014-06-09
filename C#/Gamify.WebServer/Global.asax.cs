using Gamify.Sdk.Setup;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace Gamify.WebServer
{
    public abstract class GamifyApplication : HttpApplication
    {
        private IGameDependencyModule gameDependencyModule;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            this.ConfigureDependencies();
        }

        public override void Dispose()
        {
            this.Dispose(disposing: true);
            base.Dispose();
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.gameDependencyModule != null)
                {
                    this.gameDependencyModule.Dispose();
                }
            }
        }

        protected abstract IGameDefinition GetGameDefinition();

        private void ConfigureDependencies()
        {
            var gameDefinition = this.GetGameDefinition();
            
            this.gameDependencyModule = new GameDependencyModule(gameDefinition);

            gameDependencyModule.Setup();
        }
    }
}