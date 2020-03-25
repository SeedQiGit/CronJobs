using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using CronJobsMysql.Services.Quartz.Listeners;
using Infrastructure.Extensions;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace CronJobsMysql.Services.Quartz
{
    public class QuartzService : IHostedService
    {
        #region 构造函数及字段

        private readonly IScheduler _scheduler;
        private readonly ILogger<QuartzService> _logger;

        public QuartzService(ILogger<QuartzService> logger)
        {
            var schedulerFactory = new StdSchedulerFactory(QuartzConfig());
            //var schedulerFactory = new StdSchedulerFactory(QuartzConfig());
            _scheduler = schedulerFactory.GetScheduler().Result;
            //_scheduler.ListenerManager.AddJobListener(jobListener, GroupMatcher<JobKey>.AnyGroup());
            //_scheduler.ListenerManager.AddSchedulerListener(schedulerListener);
            _logger=logger;
        }

        protected NameValueCollection QuartzConfig()
        {
            //1.首先创建一个作业调度池
            NameValueCollection properties = new NameValueCollection();
            //存储类型
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX,Quartz";
            //表明前缀
            properties["quartz.jobStore.tablePrefix"] = "qrtz_";
            //驱动类型
            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.MySQLDelegate,Quartz";
            //数据源名称
            properties["quartz.jobStore.dataSource"] = "myDS";

            //连接字符串  根本没执行这个啊
            properties["quartz.dataSource.myDS.connectionString"] = //"server=127.0.0.1;userid=root;password=123456;persistsecurityinfo=True;database=test"; 
            SettingManager.GetValue("ConnectionString");
            //版本
            properties["quartz.dataSource.myDS.provider"] = "MySql";
            properties["quartz.scheduler.instanceId"] = "AUTO";
            properties["quartz.serializer.type"] = "binary";
        
            //最大链接数
            //properties["quartz.dataSource.myDS.maxConnections"] = "5";
            // First we must get a reference to a scheduler

            return properties;
        }

        #endregion

        #region 服务启动或停止

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("QuartzService启动");

            //DateTime StarTime = DateTime.Now;
            //DateTimeOffset starRunTime = DateBuilder.NextGivenSecondDate(StarTime, 1);
            //DateTime EndTime = DateTime.MaxValue.AddDays(-1);
            //DateTimeOffset endRunTime = DateBuilder.NextGivenSecondDate(EndTime, 1);
            //IJobDetail job = JobBuilder.Create<DemoJob>()
            //    .WithIdentity("j1", "g1")
            //    .WithDescription("注释")
            //    .Build();
            //ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
            //    .StartAt(starRunTime)//指定运行时间
            //    .EndAt(endRunTime)//指定结束时间
            //    .WithIdentity("j1", "g1")
            //    .WithCronSchedule("0/5 * * * * ?")//运行模式 每十秒钟运行一次
            //    .WithDescription("注释")
            //    .Build();
            //await _scheduler.ScheduleJob(job, trigger);

            await _scheduler.Start(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("QuartzService停止");
            //这里直接关闭，不等已经执行的任务完成
            await _scheduler.Shutdown(cancellationToken);
        }

        #endregion

        
    }
    public class DemoJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            ILogger logger = ServiceProviderServiceExtensions.GetRequiredService<ILogger<DemoJob>>(
                ServiceProviderExtension.ServiceProvider);
            logger.LogInformation(string.Format("{0}执行一次", DateTime.Now));
            return  Task.CompletedTask;
        }
    }
}
