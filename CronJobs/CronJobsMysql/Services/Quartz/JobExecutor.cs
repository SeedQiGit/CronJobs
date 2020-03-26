using Microsoft.Extensions.Logging;
using Quartz;
using System.Net.Http;
using System.Threading.Tasks;

namespace CronJobsMysql.Services.Quartz
{
    public class JobExecutor : IJob
    {
        
        private readonly ILogger<JobExecutor> _logger;
        private readonly HttpClient _client;

        public JobExecutor(ILogger<JobExecutor> logger, IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _logger=logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            var url=context.JobDetail.JobDataMap["requestUrl"].ToString();
            _logger.LogInformation("请求"+url);
            var result = _client.GetAsync(url);
            return Task.CompletedTask;
        }
    }
}
