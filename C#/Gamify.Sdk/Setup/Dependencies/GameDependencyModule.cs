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

        public object Get(Type objectType)
        {
            return this.gameContainer.Resolve(objectType);
        }

        public T Get<T>()
        {
            return this.gameContainer.Resolve<T>();
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