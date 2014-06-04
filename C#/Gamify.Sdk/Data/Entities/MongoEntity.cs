using MongoDB.Bson;

namespace Gamify.Sdk.Data.Entities
{
    public abstract class MongoEntity : DataEntity<ObjectId>
    {
        protected MongoEntity()
            : base(() => new ObjectId())
        {
        }
    }
}
