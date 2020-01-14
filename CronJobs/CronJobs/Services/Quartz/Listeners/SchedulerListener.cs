using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CronJobs.Services.Quartz.Listeners
{
    public class SchedulerListener:ISchedulerListener
    {
        private readonly ILogger<SchedulerListener> _logger;
        public SchedulerListener(ILogger<SchedulerListener> logger)
        {
            _logger=logger;
        }

        public Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("任务被部署");
            return Task.CompletedTask;
        }

        public Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("任务被删除,删除时间为：" + DateTime.Now);
            return Task.CompletedTask;
        }

        public Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
             _logger.LogInformation("任务被暂停");
             return Task.CompletedTask;
        }

        public Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = default(CancellationToken))
        {
             _logger.LogInformation("任务被恢复");
             return Task.CompletedTask;
        }

        public Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobsPaused(string jobGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobsResumed(string jobGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulerError(string msg, SchedulerException cause, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        public Task SchedulerInStandbyMode(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulerShutdown(CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("调度器被关闭");
             return Task.CompletedTask;
        }

        public Task SchedulerShuttingdown(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulerStarted(CancellationToken cancellationToken = default(CancellationToken))
        {
            _logger.LogInformation("调度器被启动");
             return Task.CompletedTask;
        }

        public Task SchedulerStarting(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task SchedulingDataCleared(CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = default(CancellationToken))
        {
             _logger.LogInformation("任务完成使命，不在被执行");
             return Task.CompletedTask;
        }

        public Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggersPaused(string triggerGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task TriggersResumed(string triggerGroup, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
