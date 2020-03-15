using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using CronJobsMysql.Services.Quartz.Listeners;

namespace CronJobsMysql.Services.Quartz
{
    public class QuartzService : IHostedService
    {
        #region 构造函数及字段

        private readonly IScheduler _scheduler;
        private readonly ILogger<QuartzService> _logger;

        public QuartzService(JobListener jobListener,SchedulerListener schedulerListener,ILogger<QuartzService> logger)
        {
//            var properties = new NameValueCollection();
//            properties[StdSchedulerFactory.PropertySchedulerInstanceName] = "gogogo";
//            properties[StdSchedulerFactory.PropertySchedulerInstanceId] = "AUTO";
//            properties[StdSchedulerFactory.PropertyJobStoreType] = typeof (MongoDbJobStore).AssemblyQualifiedName;
//// I treat the database in the connection string as the one you want to connect to
//            properties[$"{StdSchedulerFactory.PropertyJobStorePrefix}.{StdSchedulerFactory.PropertyDataSourceConnectionString}"] = "mongodb://localhost/quartz";
//// The prefix is optional
//            properties[$"{StdSchedulerFactory.PropertyJobStorePrefix}.collectionPrefix"] = "prefix";

//            var scheduler = new StdSchedulerFactory(properties);
         
            var schedulerFactory = new StdSchedulerFactory(QuartzConfig());
            //var schedulerFactory = new StdSchedulerFactory(QuartzConfig());
            _scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler.ListenerManager.AddJobListener(jobListener, GroupMatcher<JobKey>.AnyGroup());
            _scheduler.ListenerManager.AddSchedulerListener(schedulerListener);
            _logger=logger;
        }

        protected NameValueCollection QuartzConfig()
        {
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteServer";
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["lazy-init"] = "false";
            properties["quartz.threadPool.threadPriority"] = "Normal";
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "665"; //ConfigurationManager.AppSettings["port"];
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";// ConfigurationManager.AppSettings["bindName"];//名称
            //通道类型
            properties["quartz.scheduler.exporter.channelType"] = "tcp";// ConfigurationManager.AppSettings["channelType"];
            properties["quartz.scheduler.exporter.channelName"] = "httpQuartz";
            properties["quartz.scheduler.exporter.rejectRemoteRequests"] = "false";
            //集群配置
            properties["quartz.jobStore.clustered"] = "true";
            //存储类型
            properties["quartz.jobStore.type"] = "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz";
            //表名前缀
            properties["quartz.jobStore.tablePrefix"] = "qrtz_";
            //驱动类型
            properties["quartz.jobStore.driverDelegateType"] = "Quartz.Impl.AdoJobStore.MySQLDelegate, Quartz";
            //数据源名称
            properties["quartz.jobStore.dataSource"] = "myDS";

            //连接字符串
            properties["quartz.dataSource.myDS.connectionString"] = "server=xxxx;userid=xxxx;password=xxxx;persistsecurityinfo=True;database=xxxx"; //ConfigurationManager.AppSettings["connectionString"];
            //版本
            properties["quartz.dataSource.myDS.provider"] = "MySql";
            properties["quartz.scheduler.instanceId"] = "AUTO";
            properties["quartz.serializer.type"] = "binary";

            return properties;
        }


        #endregion

        #region 服务启动或停止

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("QuartzService启动");
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
}
