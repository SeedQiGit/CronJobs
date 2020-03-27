using CronJobsMysql.Data.Entity;
using Quartz;

namespace CronJobsMysql.Services.Quartz.Trigger
{
    public abstract class JobBaseTrigger
    {
        protected IScheduler Scheduler { get; }=QuartzService.Scheduler;
        public abstract bool RunJob(CronJob jobInfo);

        public abstract bool ModifyCronExpress(CronJob jobInfo);

        public bool DeleteJob(CronJob jobInfo)
        {
            var jobKey = KeyManager.CreateJobKey(jobInfo.Name, jobInfo.GroupName);
            var triggerKey = KeyManager.CreateTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
            Scheduler.PauseTrigger(triggerKey);
            Scheduler.UnscheduleJob(triggerKey);
            Scheduler.DeleteJob(jobKey);
            return true;
        }

        public bool PauseJob(CronJob jobInfo)
        {
            var jobKey = KeyManager.CreateJobKey(jobInfo.Name, jobInfo.GroupName);
            Scheduler.PauseJob(jobKey);
            return true;
        }

        public bool ResumeJob(CronJob jobInfo)
        {
            var jobKey = KeyManager.CreateJobKey(jobInfo.Name, jobInfo.GroupName);
            Scheduler.ResumeJob(jobKey);
            return true;
        }
    }
}
