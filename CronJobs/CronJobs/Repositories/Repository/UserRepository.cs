using CronJobs.Data.Entity;
using CronJobs.Repositories.Repository;
using CronJobs.Repository.IRepository;
using MongoDB.Driver;

namespace CronJobs.Repository.Repository
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository(MongoClient mongoClient) : base(mongoClient)
        {
            Context = Datebase.GetCollection<User>("users");
        }

    }
}
