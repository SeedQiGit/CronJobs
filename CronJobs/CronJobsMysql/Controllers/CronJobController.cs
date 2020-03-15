using System.Collections.Generic;
using System.Threading.Tasks;
using CronJobsMysql.Data.Entity;
using CronJobsMysql.Data.Request;
using CronJobsMysql.Services.Interfaces;
using Infrastructure.Model.Response;
using Microsoft.AspNetCore.Mvc;

namespace CronJobsMysql.Controllers
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
        [ProducesResponseType(typeof(BaseResponse), 1)]
        public async Task<BaseResponse> CronJobDelete([FromBody]CronJobDeleteRequest request)
        {
            return await _cronJobService.CronJobDelete(request);
        }

        [HttpPost("CronJobUpdate")]
        [ProducesResponseType(typeof(BaseResponse), 1)]
        public async Task<BaseResponse> CronJobUpdate([FromBody]CronJobUpdateRequest request)
        {
            return await _cronJobService.CronJobUpdate(request);
        }

    }
}
