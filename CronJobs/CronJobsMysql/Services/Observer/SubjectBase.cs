using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CronJobsMysql.Data.Model;

namespace CronJobsMysql.Services.Observer
{
    public class SubjectBase
    {
        public Action<JobExcutedCallBackModel> Actions;

        protected void NotifyAsync(JobExcutedCallBackModel jobExcutedCallBackModel)
        {
            Actions(jobExcutedCallBackModel);
            //this._jobExecutedCallBack(jobExcutedCallBackModel);
        }


    }
}
