using CronJobs.Data.Entity;
using CronJobs.Data.Request;
using CronJobs.Services.Interfaces;
using Infrastructure.Model.Response;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        [HttpPost("CronJobDelete")]
        [ProducesResponseType(typeof(BaseResponse<DeleteResult>), 1)]
        public async Task<BaseResponse> CronJobDelete([FromBody]CronJobDelete request)
        {
            return await _cronJobService.CronJobDelete(request);
        }

        [HttpPost("CronJobUpdate")]
        [ProducesResponseType(typeof(BaseResponse<ReplaceOneResult>), 1)]
        public async Task<BaseResponse> CronJobUpdate([FromBody]CronJobUpdateRequest request)
        {
            return await _cronJobService.CronJobUpdate(request);
        }

    }
}
