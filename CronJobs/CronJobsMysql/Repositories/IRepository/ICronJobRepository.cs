using System.Threading.Tasks;
using BihuApiCore.Repository.IRepository;
using CronJobsMysql.Data.Entity;
using CronJobsMysql.Data.Request;
using Infrastructure.Model.Response;

namespace CronJobsMysql.Repositories.IRepository
{
    public interface ICronJobRepository: IRepositoryBase<CronJob>
    { 
        Task<BasePageResponse<CronJob>> CronJobList(CronJobListRequest request);
    }
}
