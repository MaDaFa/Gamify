using Autofac;
using System;

namespace Gamify.Sdk.Setup.Dependencies
{
    public class GameDependencyModule : IGameDependencyModule
    {
        private readonly IContainer gameContainer;

        public GameDependencyModule(IContainer gameContainer)
        {
            this.gameContainer = gameContainer;
        }

        ///<exception cref="GameSetupException">GameSetupException</exception>
        public object Get(Type objectType)
        {
            try
            {
                return this.gameContainer.Resolve(objectType);
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("An error occurred when trying to resolve game dependency for {0}. Details: {1}", objectType.Name, ex.Message);

                throw new GameSetupException(errorMessage, ex);
            }
        }

        ///<exception cref="GameSetupException">GameSetupException</exception>
        public T Get<T>()
        {
            try
            {
                return this.gameContainer.Resolve<T>();
            }
            catch (Exception ex)
            {
                var errorMessage = string.Format("An error occurred when trying to resolve game dependency for {0}. Details: {1}", typeof(T).Name, ex.Message);

                throw new GameSetupException(errorMessage, ex);
            }
        }

        public virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.gameContainer != null)
                {
                    this.gameContainer.Dispose();
                }
            }
        }

        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}