using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CronJobs.Data.Entity;
using CronJobs.Data.Request;
using CronJobs.Services.Interfaces;
using Infrastructure.Model.Response;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CronJobs.Controllers
{
    public class CronJobController:BaseController
    {
        private readonly ICronJobService _cronJobService;

        public CronJobController(ICronJobService cronJobService)
        {
            _cronJobService = cronJobService;
        }

        [HttpGet("CronJobList")]
        [ProducesResponseType(typeof(BaseResponse<List<CronJob>>), 1)]
        public async Task<BaseResponse> CronJobList([FromQuery]CronJobListRequest request)
        {
            return await _cronJobService.CronJobList(request);
        }

        [HttpPost("CronJobAdd")]
        [ProducesResponseType(typeof(BaseResponse<CronJob>), 1)]
        public async Task<BaseResponse> CronJobAdd([FromBody] CronJobAddRequest request)
        {
            return await _cronJobService.CronJobAdd(request);
        }

        //[HttpPost("Delete")]
        //public async Task<DeleteResult> Delete([FromBody]string id)
        //{
        //    return await _cronJobService.DeleteOneAsync(c=>c.Id==id);
        //}


        //[HttpPost("Update")]
        //public async Task<DeleteResult> Update([FromBody]CronJob id)
        //{
        //    return await _cronJobService.DeleteOneAsync(c=>c.Id==id);
        //}

    }
}
