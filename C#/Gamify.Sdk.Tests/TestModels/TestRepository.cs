using Gamify.Sdk.Data;
using Gamify.Sdk.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Gamify.Sdk.Tests.TestModels
{
    public class TestRepository<T> : IRepository<T>
        where T : GameEntity
    {
        private IList<T> entityList;

        public TestRepository()
        {
            this.entityList = new List<T>();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            IEnumerable<T> result = this.entityList;

            if (predicate != null)
            {
                result = result.Where(predicate.Compile());
            }

            return result.AsQueryable();
        }

        public T Get(Expression<Func<T, bool>> predicate = null)
        {
            return this.GetAll(predicate).FirstOrDefault();
        }

        public bool Exist(Expression<Func<T, bool>> predicate = null)
        {
            var existingDataObject = this.Get(predicate);

            return existingDataObject != default(T);
        }

        public void Create(T dataEntity)
        {
            this.entityList.Add(dataEntity);
        }

        public void Update(T dataEntity)
        {
            this.Delete(dataEntity);
            this.Create(dataEntity);
        }

        public void Delete(T dataEntity)
        {
            var existingEntity = this.entityList.FirstOrDefault(e => e == dataEntity);

            if (existingEntity == null)
            {
                throw new GameDataException("The entity doesn't exist");
            }

            this.entityList.Remove(existingEntity);
        }

        public void DeleteAll()
        {
            this.entityList = new List<T>();
        }
    }
}
