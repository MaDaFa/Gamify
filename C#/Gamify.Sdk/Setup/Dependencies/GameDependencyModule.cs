using Autofac;
using Gamify.Sdk.Setup.Definition;
using System;

namespace Gamify.Sdk.Setup.Dependencies
{
    public class GameDependencyModule : IGameDependencyModule
    {
        private readonly IContainer gameContainer;
        private readonly IGameDefinition gameDefinition;

        public GameDependencyModule(IContainer gameContainer, IGameDefinition gameDefinition)
        {
            this.gameContainer = gameContainer;
            this.gameDefinition = gameDefinition;
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