using CronJobs.Data.Entity;
using CronJobs.Repositories.IRepository;
using CronJobs.Repository.IRepository;
using CronJobs.Repository.Repository;
using MongoDB.Driver;

namespace CronJobs.Repositories.Repository
{
    public class CronJobRepository:BaseRepository<CronJob>, ICronJobRepository
    {
        public CronJobRepository(MongoClient mongoClient) : base(mongoClient)
        {
            Context = Datebase.GetCollection<CronJob>("CronJobs");


            var indexBuilder = Builders<CronJob>.IndexKeys;
            var keys = indexBuilder.Ascending("Name");
            var options = new CreateIndexOptions
            {
                Name = "idx_name",
            };
            var indexModel = new CreateIndexModel<CronJob>(keys, options);
            Context.Indexes.CreateOneAsync(indexModel).ConfigureAwait(false).GetAwaiter().GetResult();

            var notificationLogBuilder = Builders<CronJob>.IndexKeys;
            var indexModel1 = new CreateIndexModel<CronJob>(notificationLogBuilder.Ascending(x => x.Name));
            var a = Context.Indexes.CreateOneAsync(indexModel1).ConfigureAwait(false).GetAwaiter().GetResult();

            //var indexModel = Builders<CronJob>.IndexKeys.Ascending(_ => _.Name);
            //var a = Context.Indexes.CreateOneAsync(indexModel).ConfigureAwait(false).GetAwaiter().GetResult();
        }

    }
}
