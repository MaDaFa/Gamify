using Gamify.Data.Entities;
using MongoDB.Bson;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Gamify.Data
{
    public interface IRepository<T> where T : MongoEntity
    {
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null);

        T Get(Expression<Func<T, bool>> predicate = null);

        bool Exist(Expression<Func<T, bool>> predicate = null);

        void Create(T dataEntity);

        void Update(T dataEntity);

        void Delete(T dataEntity);

        void Delete(ObjectId id);
    }
}
