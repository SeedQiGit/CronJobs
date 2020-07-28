using System.Net.Http;
using System.Threading.Tasks;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace CronJobsMysql.Services.Quartz
{
    public class JobExecutor : IJob
    {
        //没有无参构造函数不能实例化
        //private readonly ILogger<JobExecutor> _logger;
        //private readonly HttpClient _client;

        //public JobExecutor(ILogger<JobExecutor> logger, IHttpClientFactory httpClientFactory)
        //{
        //    _client = httpClientFactory.CreateClient();
        //    _logger=logger;
        //}

        public async Task Execute(IJobExecutionContext context)
        {
            ILogger logger = ServiceProviderExtension.ServiceProvider.GetRequiredService<ILogger<JobExecutor>>();
           
            var client = ServiceProviderExtension.ServiceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();

            var url=context.JobDetail.JobDataMap["requestUrl"].ToString();

            logger.LogInformation("IJobExecutionContext" + url);

            //判断是否有BaseAddress 也就是http或者https
            //https://    http://
            url = url.Trim();
            if (!url.StartsWith("http://")&& !url.StartsWith("https://"))
            {
                url = "http://" + url;
            }
      
            var result = await client.GetAsync(url);
            logger.LogInformation("请求"+url+"返回值"+JsonConvert.SerializeObject(result));
        }
    }
}
