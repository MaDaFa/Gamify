using Gamify.Sdk.Data.Configuration;
using Gamify.Sdk.Data.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Gamify.Sdk.Data
{
    public class Repository<T> : IRepository<T>
        where T : GameEntity
    {
        private static readonly string idName = "_id";
        private static readonly string collectionName = typeof(T).Name;

        private readonly MongoDatabase database;

        public Repository(IGameDataSection configuration)
        {
            var databaseClient = new MongoClient(configuration.ConnectionString);
            var databaseServer = databaseClient.GetServer();

            this.database = databaseServer.GetDatabase(configuration.DatabaseName);
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            var collection = this.database.GetCollection<T>(collectionName).AsQueryable();

            if (predicate != null)
            {
                collection = collection.Where(predicate);
            }

            return collection;
        }

        public T Get(Expression<Func<T, bool>> predicate = null)
        {
            var filteredCollection = this.GetAll(predicate);

            return filteredCollection.FirstOrDefault();
        }

        public bool Exist(Expression<Func<T, bool>> predicate = null)
        {
            var existingDataObject = this.Get(predicate);

            return existingDataObject != default(T);
        }

        ///<exception cref="GameDataException">GameDataException</exception>
        public void Create(T dataEntity)
        {
            var collection = this.database.GetCollection<T>(collectionName);
            var insertResult = collection.Insert(dataEntity);

            if (!insertResult.Ok)
            {
                var errorMessage = string.Concat("Creation of document {0} failed", collectionName);

                throw new GameDataException(errorMessage);
            }
        }

        ///<exception cref="GameDataException">GameDataException</exception>
        public void Update(T dataEntity)
        {
            var collection = this.database.GetCollection<T>(collectionName);
            var saveResult = collection.Save(dataEntity);

            if (!saveResult.Ok)
            {
                var errorMessage = string.Concat("Update of document {0} with Id {1} failed", collectionName, dataEntity.Id);

                throw new GameDataException(errorMessage);
            }
        }

        ///<exception cref="GameDataException">GameDataException</exception>
        public void Delete(T dataEntity)
        {
            var id = dataEntity.Id;
            var collection = this.database.GetCollection<T>(collectionName);
            var removeQuery = Query.EQ(idName, id);
            var deleteResult = collection.Remove(removeQuery);

            if (!deleteResult.Ok)
            {
                var errorMessage = string.Concat("Deletion of document {0} with Id {1} failed", collectionName, id);

                throw new GameDataException(errorMessage);
            }
        }

        ///<exception cref="GameDataException">GameDataException</exception>
        public void DeleteAll()
        {
            var collection = this.database.GetCollection<T>(collectionName);
            var deleteResult = collection.RemoveAll();

            if (!deleteResult.Ok)
            {
                var errorMessage = string.Concat("Deletion of the hole collection {0} failed", collectionName);

                throw new GameDataException(errorMessage);
            }
        }
    }
}
