using System;
using CronJobsMysql.Data.Model;

namespace CronJobsMysql.Services.Observer
{
    public abstract class ObserverBase
    {
        protected ObserverBase(SubjectBase subjectBase) 
        {
            subjectBase.Actions += SendMail;
        }
     
        public abstract void SendMail(JobExcutedCallBackModel schedulerExecutedCallBackModel);
    }
}
