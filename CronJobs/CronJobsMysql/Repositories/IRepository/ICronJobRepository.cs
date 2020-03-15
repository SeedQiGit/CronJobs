using BihuApiCore.Repository.IRepository;
using CronJobsMysql.Data.Entity;

namespace CronJobsMysql.Repositories.IRepository
{
    public interface ICronJobRepository: IRepositoryBase<CronJob>
    {
        
    }
}
