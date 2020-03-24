﻿using System;
using AutoMapper;
using CronJobsMysql.Data.Request;
using CronJobsMysql.Repositories.IRepository;
using CronJobsMysql.Services.Interfaces;
using Infrastructure.Model.Response;
using System.Threading.Tasks;
using CronJobsMysql.Data.Entity;
using CronJobsMysql.Data.Enum;

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
            var res = await _cronJobRepository.CronJobList(request);
            return BaseResponse<BasePageResponse<CronJob>>.Ok(res);

        }

        public async Task<BaseResponse> CronJobAdd( CronJobAddRequest request)
        {
            //查看是否有同名定时任务
            var nameJob = await _cronJobRepository.FirstOrDefaultAsync(c => c.Name == request.Name);
            if (nameJob != null)
            {
                return BaseResponse.Failed("已有同名任务");
            }
            var cronJob = _mapper.Map<CronJob>(request);
            cronJob.CreateTime = DateTime.Now;
            cronJob.UpdateTime = DateTime.Now;
            cronJob.JobState = JobStateEnum.启用;
            
            await _cronJobRepository.InsertAsync(cronJob);
            await _cronJobRepository.SaveChangesAsync();
            return BaseResponse<CronJob>.Ok(cronJob);
        }

        public async Task<BaseResponse> CronJobDelete(CronJobDeleteRequest request)
        {
            CronJob cronJob = new CronJob(){Id=request.Id};  
            _cronJobRepository.Delete(cronJob);
            await _cronJobRepository.SaveChangesAsync();
            return BaseResponse.Ok();
        }

        public async Task<BaseResponse> CronJobUpdate(CronJobUpdateRequest request)
        {
            ////直接全更新，否可行。
            ////生产语句UPDATE `cron_job` SET `CreateTime` = @p0, `CreateUser` = @p1, `CronExpress` = @p2, `Description` = @p3, `JobState` = @p4, `Name` = @p5, `RequestUrl` = @p6, `UpdateTime` = @p7, `UpdateUser` = @p8 WHERE `Id` = @p9;
            //_cronJobRepository.Update(request.CronJob);
            //await _cronJobRepository.SaveChangesAsync();

            var nameJob = await _cronJobRepository.FirstOrDefaultAsync(c => c.Id == request.CronJob.Id);
            if (nameJob == null)
            {
                return BaseResponse.Failed("未找到对应任务");
            }

            //排除固定字段的更新  只更新指定改动的字段
            _cronJobRepository.CompareValueAndassign(nameJob,request.CronJob);
            nameJob.UpdateUser = request.UserId;
            
            await _cronJobRepository.SaveChangesAsync();

            return BaseResponse<CronJob>.Ok(nameJob);
        }
    }
}
