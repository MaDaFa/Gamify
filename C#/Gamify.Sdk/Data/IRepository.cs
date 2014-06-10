using Gamify.Sdk.Data.Entities;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Gamify.Sdk.Data
{
    public interface IRepository<T> where T : GameEntity
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null);

        T Get(Expression<Func<T, bool>> predicate = null);

        bool Exist(Expression<Func<T, bool>> predicate = null);

        void Create(T dataEntity);

        void Update(T dataEntity);

        void Delete(T dataEntity);

        void DeleteAll();
    }
}
