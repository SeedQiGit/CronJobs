using CronJobs.Data.Entity;
using CronJobs.Data.Request;
using Infrastructure.Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CronJobs.Services.Interfaces
{
    public interface ICronJobService
    {
        Task<BaseResponse<List<CronJob>>> CronJobList(CronJobListRequest request);
    }
}
