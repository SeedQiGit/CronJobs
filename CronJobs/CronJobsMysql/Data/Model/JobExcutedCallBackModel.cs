using System.Collections.Generic;

namespace CronJobsMysql.Data.Model
{
    public class JobExcutedCallBackModel
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 是否有执行异常
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 本次执行时间
        /// </summary>
        public string ExcutedTime { get; set; }

        /// <summary>
        /// 下次执行时间
        /// </summary>
        public string NextExcuteTime { get; set; }

        /// <summary>
        /// 需要发送的邮件地址  先写死
        /// </summary>
        public List<string> MailAddressList { get; set; }=new List<string> { "seedqi@126.com" };
    }
}
