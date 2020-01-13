using CronJobs.Data.Entity;
using CronJobs.Repository.IRepository;
using MongoDB.Driver;

namespace CronJobs.Repository.Repository
{
    public class CronJobRepository:BaseRepository<CronJob>, ICronJobRepository
    {
        public CronJobRepository(MongoClient mongoClient) : base(mongoClient)
        {
            Context = Datebase.GetCollection<CronJob>("CronJobs");
        }

    }
}
