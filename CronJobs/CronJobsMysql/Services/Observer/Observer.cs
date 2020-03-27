using CronJobsMysql.Data.Model;
using Infrastructure.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CronJobsMysql.Services.Observer
{
    public class Observer : ObserverBase
    {

        public Observer(SubjectBase subjectBase) : base(subjectBase)
        {
          
        }

       

        public override void SendMail(JobExcutedCallBackModel schedulerExecutedCallBackModel)
        {
            ILogger logger = ServiceProviderExtension.ServiceProvider.GetRequiredService<ILogger<Observer>>();
            logger.LogInformation("观察者模式发送邮件：");
        }
    }
}
