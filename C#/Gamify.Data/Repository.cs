using Gamify.Data.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Gamify.Data
{
    public class Repository<T> : IRepository<T>
        where T : DataObject
    {
        private static readonly string idName = "_id";
        private static readonly string collectionName = typeof(T).Name;

        private readonly MongoDatabase database;

        public Repository(IGameDataConfiguration configuration)
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

        public T Get(Guid id)
        {
            var collection = this.database.GetCollection<T>(collectionName);

            return collection.FindOneByIdAs<T>(id);
        }

        ///<exception cref="GameDataException">GameDataException</exception>
        public Guid Create(T dataObject)
        {
            var collection = this.database.GetCollection<T>(collectionName);
            var insertResult = collection.Insert(dataObject);

            if (!insertResult.Ok)
            {
                var errorMessage = string.Concat("Creation of document {0} failed", collectionName);

                throw new GameDataException(errorMessage);
            }

            return dataObject.Id;
        }

        ///<exception cref="GameDataException">GameDataException</exception>
        public void Update(T dataObject)
        {
            var collection = this.database.GetCollection<T>(collectionName);
            var saveResult = collection.Save(dataObject);

            if (!saveResult.Ok)
            {
                var errorMessage = string.Concat("Update of document {0} with Id {1} failed", collectionName, dataObject.Id);

                throw new GameDataException(errorMessage);
            }
        }

        ///<exception cref="GameDataException">GameDataException</exception>
        public void Delete(T dataObject)
        {
            this.Delete(dataObject.Id);
        }

        ///<exception cref="GameDataException">GameDataException</exception>
        public void Delete(Guid id)
        {
            var collection = this.database.GetCollection<T>(collectionName);
            var removeQuery = Query.EQ(idName, id);
            var deleteResult = collection.Remove(removeQuery);

            if (!deleteResult.Ok)
            {
                var errorMessage = string.Concat("Delete of document {0} with Id {1} failed", collectionName, id);

                throw new GameDataException(errorMessage);
            }
        }
    }
}
