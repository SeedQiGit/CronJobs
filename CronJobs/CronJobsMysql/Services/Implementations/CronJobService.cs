using AutoMapper;
using CronJobs.Data.Entity;
using CronJobs.Data.Enum;
using CronJobs.Data.Request;
using CronJobsMysql.Services.Interfaces;
using Infrastructure.Model.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CronJobsMysql.Services.Implementations
{
    public class CronJobService:ICronJobService
    {
        private readonly ICronJobRepository _cronJobRepository;
        private readonly IMapper _mapper;

        public CronJobService(ICronJobRepository cronJobRepository,IMapper mapper)
        {
            _cronJobRepository = cronJobRepository;
            _mapper=mapper;
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

        public async Task<BaseResponse> CronJobAdd( CronJobAddRequest request)
        {
            //查看是否有同名定时任务
            var nameJob = await _cronJobRepository.FirstOrDefaultAsync(c=>c.Name==request.Name);
            if (nameJob!=null)
            {
                return BaseResponse.Failed("已有同名任务");
            }
            var cronJob = _mapper.Map<CronJob>(request);
            cronJob.CreateTime=DateTime.Now;
            cronJob.UpdateTime=DateTime.Now;
            cronJob.JobState=JobStateEnum.启用;
            await _cronJobRepository.AddAsync(cronJob);
            
            return BaseResponse<CronJob>.Ok(cronJob);
        }

        public async Task<BaseResponse> CronJobDelete(CronJobDeleteRequest request)
        {
            var deleteResult =await _cronJobRepository.DeleteOneAsync(x => x.Id == request.Id);

            return BaseResponse<DeleteResult>.Ok(deleteResult);
        }

        public async Task<BaseResponse> CronJobUpdate(CronJobUpdateRequest request)
        {
            //直接使用替换模式
            //var update =Builders<CronJob>.Update.Set("UpdateTime",DateTime.Now);
            //if (request.Description!=null)
            //{
            //    update.Set("Description", request.Description);
            //}

            request.CronJob.UpdateTime=DateTime.Now;
   
            //直接使用替换
            var replaceOneResult =await _cronJobRepository.ReplaceOneAsync(request.CronJob);

            return BaseResponse<ReplaceOneResult>.Ok(replaceOneResult);

        }
    }
}
