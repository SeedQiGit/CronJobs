using CronJobs.Data.Entity;
using CronJobs.Data.Request;
using CronJobs.Repository.IRepository;
using CronJobs.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Model.Response;

namespace CronJobs.Services.Implementations
{
    public class CronJobService:ICronJobService
    {
        private readonly ICronJobRepository _cronJobRepository;

        public CronJobService(ICronJobRepository cronJobRepository)
        {
            _cronJobRepository = cronJobRepository;
        }

        public async  Task<BaseResponse<List<CronJob>>> CronJobList(CronJobListRequest request)
        {

            
            return await _userRepository.GetListAsync();

            return BaseResponse<List<CronJob>>.Ok(new List<CronJob>());
        }
    }
}
