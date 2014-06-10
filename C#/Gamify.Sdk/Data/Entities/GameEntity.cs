using System;

namespace Gamify.Sdk.Data.Entities
{
    public abstract class GameEntity : DataEntity<Guid>
    {
        protected GameEntity()
            : base(() => Guid.NewGuid())
        {
        }
    }
}
