using System.Threading.Tasks;
using CronJobsMysql.Data.Request;
using Infrastructure.Model.Response;

namespace CronJobsMysql.Services.Interfaces
{
    public interface ICronJobService
    {
        Task<BaseResponse> CronJobList(CronJobListRequest request);
        Task<BaseResponse> CronJobAdd( CronJobAddRequest request);
        Task<BaseResponse> CronJobDelete(CronJobDeleteRequest request);
        Task<BaseResponse> CronJobUpdate(CronJobUpdateRequest request);

        Task<BaseResponse> ModifyCronExpress(ModifyCronExpressRequest request);
        Task<BaseResponse> CronJobState(CronJobStateRequest request);
    }
}
