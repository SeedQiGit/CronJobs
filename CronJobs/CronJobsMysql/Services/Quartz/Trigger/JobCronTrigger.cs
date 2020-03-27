using CronJobsMysql.Data.Entity;
using Quartz;
using System;

namespace CronJobsMysql.Services.Quartz.Trigger
{
    public class JobCronTrigger:JobBaseTrigger
    {

        public override bool RunJob(CronJob jobInfo)
        {
            var jobKey = KeyManager.CreateJobKey(jobInfo.Name, jobInfo.GroupName);

            if (!Scheduler.CheckExists(jobKey).Result)
            {
                //创建工作
                IJobDetail jobDetail = JobBuilder.Create<JobExecutor>().WithIdentity(jobKey).UsingJobData(KeyManager.CreateJobDataMap("requestUrl", jobInfo.RequestUrl)).Build();

                //创建事件表
                CronScheduleBuilder cronScheduleBuilder = CronScheduleBuilder.CronSchedule(jobInfo.CronExpress);
                ITrigger trigger = TriggerBuilder.Create().StartAt(DateTimeOffset.Now.AddYears(-1))
                    .WithIdentity(jobInfo.TriggerName, jobInfo.TriggerGroupName)
                    .ForJob(jobKey)
                    .WithSchedule(cronScheduleBuilder.WithMisfireHandlingInstructionDoNothing())
                    .Build();
                Scheduler.ScheduleJob(jobDetail, trigger);
            }

            return true;
        }

        public override bool ModifyCronExpress(CronJob jobInfo)
        {
            var scheduleBuilder = CronScheduleBuilder.CronSchedule(jobInfo.CronExpress);
            var triggerKey = KeyManager.CreateTriggerKey(jobInfo.TriggerName, jobInfo.TriggerGroupName);

            var trigger = TriggerBuilder.Create().StartAt(DateTimeOffset.Now.AddYears(-1)).WithIdentity(triggerKey).WithSchedule(scheduleBuilder.WithMisfireHandlingInstructionDoNothing()).Build();
            //WithMisfireHandlingInstructionDoNothing 不奏效

            //Remove (delete) the Quartz.ITrigger with the given key, and store the new given one
            Scheduler.RescheduleJob(triggerKey, trigger);
            return true;
        }

    }
}
