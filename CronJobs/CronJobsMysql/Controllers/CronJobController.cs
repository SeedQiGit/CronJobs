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

        [HttpPost("CronJobList")]
        [ProducesResponseType(typeof(BasePageResponse<CronJob>), 1)]
        public async Task<BaseResponse> CronJobList([FromBody]CronJobListRequest request)
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

        [HttpPost("CronJobState")]
        [ProducesResponseType(typeof(BaseResponse), 1)]
        public async Task<BaseResponse> CronJobState([FromBody]CronJobStateRequest request)
        {
            return await _cronJobService.CronJobState(request);
        }

        [HttpPost("ModifyCronExpress")]
        [ProducesResponseType(typeof(BaseResponse), 1)]
        public async Task<BaseResponse> ModifyCronExpress([FromBody]ModifyCronExpressRequest request)
        {
            return await _cronJobService.ModifyCronExpress(request);
        }

        ///// <summary>
        ///// 更新，暂时不实用，而是直接暂停或者修改时间周期
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //[HttpPost("CronJobUpdate")]
        //[ProducesResponseType(typeof(BaseResponse), 1)]
        //public async Task<BaseResponse> CronJobUpdate([FromBody]CronJobUpdateRequest request)
        //{
        //    return await _cronJobService.CronJobUpdate(request);
        //}




    }
}
