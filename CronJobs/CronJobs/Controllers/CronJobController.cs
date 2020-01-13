using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CronJobs.Data.Entity;
using CronJobs.Data.Request;
using CronJobs.Services.Interfaces;
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

        [HttpGet("List")]
        public async Task<List<CronJob>> CronJobList(CronJobListRequest request)
        {
            return await _cronJobService.GetListAsync();
        }

        [HttpPost("Add")]
        public async Task<string> Add([FromBody] CronJob user)
        { 
            await _cronJobService.AddAsync(user);
            return user.Id;
        }

        [HttpPost("Delete")]
        public async Task<DeleteResult> Delete([FromBody]string id)
        {
            return await _cronJobService.DeleteOneAsync(c=>c.Id==id);
        }

        
        [HttpPost("Update")]
        public async Task<DeleteResult> Update([FromBody]CronJob id)
        {
            return await _cronJobService.DeleteOneAsync(c=>c.Id==id);
        }

    }
}
