using CronJobs.Data.Entity;
using CronJobs.Repositories.IRepository;

namespace CronJobs.Repository.IRepository
{
    public interface IUserRepository: IBaseRepository<User>
    {
    }
}
