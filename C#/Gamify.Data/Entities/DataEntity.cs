using System;

namespace Gamify.Data.Entities
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
