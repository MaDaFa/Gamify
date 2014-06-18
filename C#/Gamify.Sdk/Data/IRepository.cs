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

        ///<exception cref="GameDataException">GameDataException</exception>
        void Create(T dataEntity);

        ///<exception cref="GameDataException">GameDataException</exception>
        void Update(T dataEntity);

        ///<exception cref="GameDataException">GameDataException</exception>
        void Delete(T dataEntity);

        ///<exception cref="GameDataException">GameDataException</exception>
        void DeleteAll();
    }
}
