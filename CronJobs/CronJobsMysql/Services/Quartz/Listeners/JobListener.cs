using System;
using Quartz;
using System.Threading;
using System.Threading.Tasks;
using CronJobsMysql.Data.Model;
using CronJobsMysql.Services.Observer;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CronJobsMysql.Services.Quartz.Listeners
{
    public class JobListener:SubjectBase,IJobListener
    {
        public JobListener()
        {
            new Observer.Observer(this);
        }

        public string Name => "JobListener";

        
        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
          
            return Task.CompletedTask;
        }

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            ILogger logger = ServiceProviderExtension.ServiceProvider.GetRequiredService<ILogger<JobListener>>();
            long jobId = Convert.ToInt64(context.JobDetail.JobDataMap["jobId"]);
            logger.LogInformation("JobWasExecuted:"+jobId);

            var jobName = context.JobDetail.Key.Name;
          
            string exceptionMessage = jobException == null ? null : jobException.Message;


            var preTime = context.Trigger.GetPreviousFireTimeUtc().HasValue ? context.Trigger.GetPreviousFireTimeUtc()?.LocalDateTime as DateTime? : null;
            var nextTime = context.Trigger.GetNextFireTimeUtc().HasValue ? context.Trigger.GetNextFireTimeUtc()?.LocalDateTime as DateTime? : null ;


            this.NotifyAsync(new JobExcutedCallBackModel { JobName = jobName, RequestUrl = context.JobDetail.JobDataMap["requestUrl"].ToString(), });

            return Task.CompletedTask;
        }
    }
}
