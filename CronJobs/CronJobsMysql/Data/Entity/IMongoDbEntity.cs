using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CronJobs.Data.Entity
{
    public interface IMongoDbEntity
    {
        string Id { get; set; }
    }
}
