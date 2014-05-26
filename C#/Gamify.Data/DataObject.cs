using System;

namespace Gamify.Data
{
    public abstract class DataObject
    {
        public Guid Id { get; set; }

        protected DataObject()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
