using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;

namespace CronJobsMysql.Services.Quartz.Trigger
{
    public class JobBaseTrigger
    {
        protected IScheduler Scheduler { get; set; }
        //public abstract bool RunJob(customer_quartzjobinfo jobInfo);
        //public abstract bool ModifyJobCron(customer_quartzjobinfo jobInfo);
        //public bool DeleteJob(customer_quartzjobinfo jobInfo)
        //{
        //    var jobKey = KeyManager.CreateJobKey(jobInfo.JobName, jobInfo.JobGroupName);
        //    var triggerKey = KeyManager.CreateTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);
        //    Scheduler.PauseTrigger(triggerKey);
        //    Scheduler.UnscheduleJob(triggerKey);
        //    Scheduler.DeleteJob(jobKey);
        //    return true;
        //}

        //public bool PauseJob(customer_quartzjobinfo jobInfo)
        //{
        //    var jobKey = KeyManager.CreateJobKey(jobInfo.JobName, jobInfo.JobGroupName);
        //    Scheduler.PauseJob(jobKey);
        //    return true;
        //}
        //public bool ResumeJob(customer_quartzjobinfo jobInfo)
        //{
        //    var jobKey = KeyManager.CreateJobKey(jobInfo.JobName, jobInfo.JobGroupName);
        //    Scheduler.ResumeJob(jobKey);
        //    return true;
        //}

    }
}
