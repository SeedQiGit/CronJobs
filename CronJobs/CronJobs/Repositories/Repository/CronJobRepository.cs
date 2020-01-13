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
        }

    }
}
