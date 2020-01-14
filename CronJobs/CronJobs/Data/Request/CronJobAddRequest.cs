using Infrastructure.Model.Request;
using System.ComponentModel.DataAnnotations;

namespace CronJobs.Data.Request
{
    public class CronJobAddRequest:BaseRequest
    {
        /// <summary>
        /// 任务名称
        /// </summary>
        [Required] 
        public string Name { get; set; }

        /// <summary>
        /// 任务描述信息
        /// </summary>
        [Required]  
        public string Description { get; set; }

        /// <summary>
        /// 运行周期表达式
        /// </summary>           
        [Required] 
        public string CronExpress { get; set; }

        /// <summary>
        /// 任务请求得业务地址（目前只支持get请求）
        /// </summary>           
        [Required] 
        public string RequestUrl { get; set; }


    }
}
