using CronJobs.Data.Request;
using Infrastructure.Model.Response;
using System.Threading.Tasks;

namespace CronJobs.Services.Interfaces
{
    public interface ICronJobService
    {
        Task<BaseResponse> CronJobList(CronJobListRequest request);
        Task<BaseResponse> CronJobAdd( CronJobAddRequest request);
    }
}
