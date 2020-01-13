using CronJobs.Data.Entity;
using CronJobs.Data.Request;
using CronJobs.Repositories.IRepository;
using CronJobs.Services.Interfaces;
using Infrastructure.Model.Response;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CronJobs.Services.Implementations
{
    public class CronJobService:ICronJobService
    {
        private readonly ICronJobRepository _cronJobRepository;

        public CronJobService(ICronJobRepository cronJobRepository)
        {
            _cronJobRepository = cronJobRepository;
        }

        public async Task<BaseResponse> CronJobList(CronJobListRequest request)
        {
            var filterBuilder = Builders<CronJob>.Filter;
            var filter = filterBuilder.Empty;

            if (!string.IsNullOrEmpty(request.Name))
            {
                filter=filter&filterBuilder.Eq("Name",request.Name);
            }

            if (request.JobState!=0)
            {
                filter=filter&filterBuilder.Eq("JobState",request.JobState);
            }

            var sort=request.OrderBy==0 ? Builders<CronJob>.Sort.Ascending(request.OrderByField) : Builders<CronJob>.Sort.Descending(request.OrderByField);

            var list = await _cronJobRepository.GetListAsync(filter,request.Skip,request.PageSize,sort);

            return BaseResponse<List<CronJob>>.Ok(list);
        }
    }
}
