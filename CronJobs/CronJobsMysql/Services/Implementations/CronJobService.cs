﻿using System;
using AutoMapper;
using CronJobsMysql.Data.Request;
using CronJobsMysql.Repositories.IRepository;
using CronJobsMysql.Services.Interfaces;
using Infrastructure.Model.Response;
using System.Threading.Tasks;
using CronJobsMysql.Data.Entity;
using CronJobsMysql.Data.Enum;
using CronJobsMysql.Services.Quartz.Trigger;

namespace CronJobsMysql.Services.Implementations
{
    public class CronJobService:ICronJobService
    {
        private readonly ICronJobRepository _cronJobRepository;
        private readonly IMapper _mapper;
        private readonly JobCronTrigger _jobCronTrigger;

        public CronJobService(ICronJobRepository cronJobRepository,IMapper mapper,JobCronTrigger jobCronTrigger)
        {
            _jobCronTrigger=jobCronTrigger;
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

            if (_jobCronTrigger.RunJob(cronJob))
            {
                return BaseResponse<CronJob>.Ok(cronJob);
            }
            return BaseResponse.Failed();
        }

        public async Task<BaseResponse> CronJobDelete(CronJobDeleteRequest request)
        {
            //CronJob cronJob = new CronJob(){Id=request.Id};  
            //_cronJobRepository.Delete(cronJob);
            //await _cronJobRepository.SaveChangesAsync();

            var response=await _operateJob(request.Id, async cronJob =>
            {
                _cronJobRepository.Delete(cronJob);
                await _cronJobRepository.SaveChangesAsync();
                return _jobCronTrigger.DeleteJob(cronJob);
            });

            return response;
        }

        public async Task<BaseResponse> ModifyCronExpress(ModifyCronExpressRequest request)
        {
            //var cronJob = await _cronJobRepository.FirstOrDefaultAsync(c => c.Id == request.Id);
            //if (cronJob == null)
            //{
            //    return BaseResponse.Failed("未找到对应任务");
            //}
            //cronJob.CronExpress=request.CronExpress;
            //await _cronJobRepository.SaveChangesAsync();
            //_jobCronTrigger.ModifyCronExpress(cronJob);
            //return BaseResponse.Ok();

            return await _operateJob(request.Id, async (jobDetail) =>
            {
                jobDetail.CronExpress =request.CronExpress;
                await _cronJobRepository.SaveChangesAsync(); 
                return _jobCronTrigger.ModifyCronExpress(jobDetail);
            });
        }

        public async Task<BaseResponse> CronJobState(CronJobStateRequest request)
        {
            return await _operateJob(request.Id, async (jobDetail) =>
            {
                jobDetail.JobState =request.JobState;
                await _cronJobRepository.SaveChangesAsync();
                if (request.JobState==JobStateEnum.启用)
                {
                    return _jobCronTrigger.ResumeJob(jobDetail);
                }
                return _jobCronTrigger.PauseJob(jobDetail);
            });
        }

        /// <summary>
        /// 不使用
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 操作任务
        /// </summary>
        /// <param name="jobId">任务编号</param>
        /// <param name="operateJobFunc">具体操作任务的委托</param>
        /// <returns></returns>
        private async Task<BaseResponse> _operateJob(long jobId, Func<CronJob, Task<bool>> operateJobFunc)
        {
            var jobDetail = await _cronJobRepository.FirstOrDefaultAsync(c=>c.Id==jobId);
            if (jobDetail == null)
            {
                return BaseResponse.Failed("未找到对应任务");
            }
            else
            {
                //_setSpecificTrigger(jobDetail.TriggerType);
                var isSuccess =await operateJobFunc(jobDetail);
                if (isSuccess)
                {
                    return BaseResponse.Ok();
                }
                return BaseResponse.Failed();
            }
        }

        //private void _setSpecificTrigger(string triggerType)
        //{
        //    _triggerBase = _triggerBases.FirstOrDefault(x => x.GetType().Name == triggerType);
        //}
    }
}
