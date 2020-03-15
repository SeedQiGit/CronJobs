using CronJobsMysql.Data.Entity;
using CronJobsMysql.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CronJobsMysql.Repositories.Repositories
{
    public class CronJobRepository: EfRepositoryBase<CronJob>, ICronJobRepository
    {
        public CronJobRepository(DbContext context) : base(context)
        {
        }
    }
}

