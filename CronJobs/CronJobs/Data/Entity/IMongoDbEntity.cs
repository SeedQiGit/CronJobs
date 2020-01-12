using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CronJobs.Data.Entity
{
    public interface IMongoDbEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        string Id { get; set; }
    }
}
