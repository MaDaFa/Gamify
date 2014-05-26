using System;
using System.Linq;
using System.Linq.Expressions;

namespace Gamify.Data
{
    public interface IRepository<T> where T : DataObject
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null);

        T Get(Expression<Func<T, bool>> predicate = null);

        T Get(Guid id);

        Guid Create(T dataObject);

        void Update(T dataObject);

        void Delete(T dataObject);

        void Delete(Guid id);
    }
}
