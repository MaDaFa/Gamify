using System;

namespace Gamify.Sdk.Data.Entities
{
    public abstract class DataEntity<T>
    {
        public T Id { get; set; }

        protected DataEntity()
        {
        }

        protected DataEntity(Func<T> idGenerator)
        {
            this.Id = idGenerator.Invoke();
        }
    }
}
